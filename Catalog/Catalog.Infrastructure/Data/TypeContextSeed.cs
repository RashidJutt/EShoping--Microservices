using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public class TypeContextSeed
{
    public static void SeedData(IMongoCollection<ProductType> typeCollection)
    {
        var checkTypes = typeCollection.Find(_ => true).Any();
        var relativePath = Path.Combine("Data", "SeedData", "types.json");
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        if (!checkTypes)
        {
            var typesData = File.ReadAllText(fullPath);
            var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            if (types != null)
            {
                foreach (var type in types)
                {
                    typeCollection.InsertOneAsync(type);
                }
            }
        }
    }
}
