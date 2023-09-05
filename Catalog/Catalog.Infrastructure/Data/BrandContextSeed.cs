using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public static class BrandContextSeed
{
    public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
    {
        var checkBrands=brandCollection.Find(_=> true).Any();
        var relativePath = Path.Combine("Data", "SeedData", "brands.json");
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        if (!checkBrands)
        {
            var brandsData=File.ReadAllText(fullPath);
            var brands=JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            if (brands != null)
            {
                foreach(var brand in brands)
                {
                    brandCollection.InsertOneAsync(brand);
                }
            }
        }
    }
}
