using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository, IBrandRepository, ITypesRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }
    public async Task<Product> CreateProduct(Product product)
    {
        await _context.Products.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        DeleteResult result = await _context.Products.DeleteOneAsync(filter);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _context.Brands.Find(b => true).ToListAsync();
    }

    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        return (await _context.Types.FindAsync(_ => true)).ToList();
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByBrand(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);
        return await _context.Products.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await _context.Products.Find(filter).ToListAsync();
    }

    public async Task<Pagination<Product>> GetProducts(CatalogSpecsParams catalogSpecsParams)
    {
        FilterDefinitionBuilder<Product> builder = Builders<Product>.Filter;
        FilterDefinition<Product> filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(catalogSpecsParams.Search))
        {
            var searchFilter = builder.Regex(p => p.Name, new BsonRegularExpression(catalogSpecsParams.Search));
            filter &= searchFilter;
        }
        if (!string.IsNullOrWhiteSpace(catalogSpecsParams.BrandId))
        {
            var brandFilter = builder.Eq(p => p.Brands.Id, catalogSpecsParams.BrandId);
            filter &= brandFilter;
        }
        if (!string.IsNullOrWhiteSpace(catalogSpecsParams.TypeId))
        {
            var typeFilter = builder.Eq(p => p.Types.Id, catalogSpecsParams.TypeId);
            filter &= typeFilter;
        }

        if (!string.IsNullOrWhiteSpace(catalogSpecsParams.Sort))
        {
            return new Pagination<Product>
            {
                PageIndex = catalogSpecsParams.PageIndex,
                PageSize = catalogSpecsParams.PageSize,
                Data = await DataFilter(catalogSpecsParams, filter),
                Count = await _context.Products.CountDocumentsAsync(_ => true)

            };
        }

        return new Pagination<Product>
        {
            PageIndex = catalogSpecsParams.PageIndex,
            PageSize = catalogSpecsParams.PageSize,
            Data = await _context.Products
            .Find(filter)
            .Sort(Builders<Product>.Sort.Ascending(p => p.Name))
            .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
            .Limit(catalogSpecsParams.PageSize)
            .ToListAsync(),
            Count = await _context.Products.CountDocumentsAsync(_ => true)

        };
    }

    private async Task<List<Product>> DataFilter(CatalogSpecsParams catalogSpecsParams, FilterDefinition<Product> filter)
    {

        switch (catalogSpecsParams.Sort)
        {
            case "priceAsc":
                return await _context.Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending(p => p.Price))
                    .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
                    .Limit(catalogSpecsParams.PageSize)
                    .ToListAsync();
            case "priceDesc":
                return await _context.Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Descending(p => p.Price))
                    .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
                    .Limit(catalogSpecsParams.PageSize)
                    .ToListAsync();
            default:
                return await _context.Products
                    .Find(filter)
                    .Sort(Builders<Product>.Sort.Ascending(p => p.Name))
                    .Skip(catalogSpecsParams.PageSize * (catalogSpecsParams.PageIndex - 1))
                    .Limit(catalogSpecsParams.PageSize)
                    .ToListAsync();
        }
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        ReplaceOneResult result = await _context.Products.ReplaceOneAsync(p => p.Id == product.Id, product);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
}
