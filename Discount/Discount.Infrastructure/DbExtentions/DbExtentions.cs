using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.DbExtentions;

public static class DbExtentions
{
    public static IHost MigrateDatabase<TContext>(this IHost host)
    {

        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();

        try
        {
            logger.LogInformation("Discount Database Migration started");
            ApplyMigration(configuration);
            logger.LogInformation("Discount Migraiton completed");
        }
        catch (Exception ex)
        {
            logger.LogError("Error occour while migrating discount database", ex.Message);
            throw;
        }

        return host;
    }

    private static void ApplyMigration(IConfiguration configuration)
    {
        using var connection = new NpgsqlConnection(configuration.GetSection("DatabaseSettings:ConnectionString").Value);
        connection.Open();
        using var command = new NpgsqlCommand() { Connection = connection };
        command.CommandText = "DROP TABLE IF EXISTS Coupon";
        command.ExecuteNonQuery();
        command.CommandText = @"CREATE TABLE Coupon ( Id SERIAL PRIMARY KEY,
                                                      ProductName VARCHAR(500) NOT NULL,
                                                      Description TEXT,
                                                      Amount INT)";
        command.ExecuteNonQuery();
        command.CommandText = @"INSERT INTO Coupon(ProductName,Description,Amount) Values ('Adding Quick Force Indoor Badminton Shoes','Shoe Discount',500) ";
        command.ExecuteNonQuery();
        command.CommandText = @"INSERT INTO Coupon(ProductName,Description,Amount) Values ('Yonex VCORE Pro 100 A Tennis Recquet (270gm, Strung)','Recquet Discount',700) ";
        command.ExecuteNonQuery();
    }
}
