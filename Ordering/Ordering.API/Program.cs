using Common.Logging;
using Ordering.API.Extentions;
using Ordering.Infrastructure.Data;
using Serilog;

namespace Ordering.API;
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
           .Build()
           .MigrateDatabase<OrderDbContext>(async (context, services) =>
           {
               var logger = services.GetService<ILogger<OrderContextSeed>>();
               logger.LogInformation("Calling Seed Method");
               await OrderContextSeed.SeedAsync(context, logger!);
           })
           .Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder.UseStartup<Startup>();
              }).UseSerilog(Logging.ConfigureLogger);
}