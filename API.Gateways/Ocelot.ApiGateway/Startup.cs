using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace Ocelot.ApiGateway;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var authScheme = "AuthSchemeGateway";
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(authScheme, options =>
            {
                options.Authority = "https://localhost:9009";
                options.Audience = "EShopingGateway";
            });

        services.AddOcelot()
            .AddCacheManager(o => o.WithDictionaryHandle());
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello Ocelot"); });
        });
        await app.UseOcelot();
    }
}