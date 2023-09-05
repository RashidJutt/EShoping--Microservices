using Amazon.Runtime.Internal.Util;
using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }

    public IMongoCollection<ProductBrand> Brands { get; }

    public IMongoCollection<ProductType> Types { get; }

    public CatalogContext(IConfiguration configuration)
    {
        
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSetting:DatabaseName"));
       
        Brands = database.GetCollection<ProductBrand>(configuration.GetValue<string>("DatabaseSetting:BrandsCollection"));
        Types = database.GetCollection<ProductType>(configuration.GetValue<string>("DatabaseSetting:TypesCollection"));
        Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSetting:CollectionName"));

        BrandContextSeed.SeedData(Brands);
        TypeContextSeed.SeedData(Types);
        CatalogContextSeed.SeedData(Products);
    }
}
