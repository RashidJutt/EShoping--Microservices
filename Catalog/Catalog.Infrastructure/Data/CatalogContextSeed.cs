using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection)
    {
        var checkProducts = productCollection.Find(_ => true).Any();
        var relativePath = Path.Combine("Data", "SeedData", "products.json");
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        if (!checkProducts)
        {
            var productsData = File.ReadAllText(fullPath);
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products != null)
            {
                foreach (var product in products)
                {
                    productCollection.InsertOneAsync(product);
                }
            }
        }
    }
}
