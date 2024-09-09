using Common.Logging.Correlation;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;

namespace Discount.API;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(CreateDiscountCommandHandler).Assembly));

        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

        services.AddAutoMapper(typeof(Startup));
        services.AddGrpc();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<DiscountService>();
            endpoints.MapGet("/", async (context) =>
            {
                await context.Response.WriteAsync("Grpc Service must be utilized from grpc client");
            });
        });
    }
}
