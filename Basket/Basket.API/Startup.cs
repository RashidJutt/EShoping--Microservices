﻿using Basket.Application.GrpcServices;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using DiscountAPI;
using HealthChecks.UI.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;
namespace Basket.API;

public class Startup
{
    public IConfiguration Configuration;
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiVersioning();

        //Redis Setup
        services.AddStackExchangeRedisCache(setup =>
        {
            setup.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");
        });

        services.AddMediatR(typeof(CreateShoppingCartCommandHandler).GetTypeInfo().Assembly);
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddAutoMapper(typeof(Startup));
        services.AddScoped<DiscountGrpcService>();
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(x => x.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));
        services.AddSwaggerGen(setup => setup.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Basket API", Version = "v1" }));
        services.AddHealthChecks()
            .AddRedis(Configuration.GetValue<string>("CacheSettings:ConnectionString"), "Redis Health", HealthStatus.Degraded);

        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, con) =>
            {
                con.Host(Configuration["EventBussSettings:HostAddress"]);
            });
        });

        services.AddMassTransitHostedService();

        //Add Authorization Filter.
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        services.AddControllers(options =>
        {
            options.Filters.Add(new AuthorizeFilter(policy));
        });

        //Add Authentication 
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.Authority = "https://localhost:9009";
                 options.Audience = "Basket";
             });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket Api v1");
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(configure =>
            {
                configure.MapControllers();
                configure.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

        }
    }
}
