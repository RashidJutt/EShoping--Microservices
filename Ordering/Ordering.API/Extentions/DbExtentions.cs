using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extentions;

public static class DbExtentions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var logger = serviceProvider.GetRequiredService<ILogger<TContext>>();
        var context = serviceProvider.GetRequiredService<TContext>();

        try
        {
            logger.LogInformation("Database migratin started: {Name}", typeof(TContext).Name);

            CallSeeder(seeder!, context!, serviceProvider);

            logger.LogInformation("Database migration complete: {Name}", typeof(TContext).Name);
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "An error occurred in executing db migrations: {Name}", typeof(TContext).Name);
        }

        return host;
    }

    private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider serviceProvider) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, serviceProvider);
    }
}
