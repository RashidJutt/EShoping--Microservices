using Common.Logging;
using Discount.API;
using Discount.Infrastructure.DbExtentions;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.MigrateDatabase<Program>();
        host.Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>();
            }).UseSerilog(Logging.ConfigureLogger); ;
    }
}