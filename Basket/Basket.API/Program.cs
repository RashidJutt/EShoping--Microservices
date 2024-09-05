using Common.Logging;
using Serilog;

namespace Basket.API;
public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(configure =>
        {
            configure.UseStartup<Startup>();
        }).UseSerilog(Logging.ConfigureLogger);
}