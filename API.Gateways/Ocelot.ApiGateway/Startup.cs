﻿using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Common.Logging.Correlation;
namespace Ocelot.ApiGateway;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ICorrelationIdGenerator,CorrelationIdGenerator>();
        services.AddOcelot()
            .AddCacheManager(o => o.WithDictionaryHandle());
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.AddCorrelationIdMiddleware();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello Ocelot"); });
        });
        await app.UseOcelot();
    }
}