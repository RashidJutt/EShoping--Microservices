using Common.Logging.Correlation;
using EventBuss.Messages.common;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.OpenApi.Models;
using Ordering.API.Consumers;
using Ordering.Application.Extentions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extentions;

namespace Ordering.API;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApiVersioning();
        services.AddApplicationService();
        services.AddInfraServices(Configuration);
        services.AddAutoMapper(typeof(Startup));
        services.AddScoped<BasketOrderingConsumer>();
        services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Ordering.Api", Version = "v1" });
        });

        services.AddHealthChecks().Services.AddDbContext<OrderDbContext>();
        services.AddMassTransit(config =>
        {
            config.AddConsumer<BasketOrderingConsumer>();
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.ReceiveEndpoint(EventBussConstants.BasketCheckoutQueue, config =>
                {
                    config.ConfigureConsumer<BasketOrderingConsumer>(ctx);
                });
                cfg.Host(Configuration["EventBussSettings:HostAddress"]);
            });
        });

        services.AddMassTransitHostedService();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.Api v1");
            });

            app.UseRouting();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
