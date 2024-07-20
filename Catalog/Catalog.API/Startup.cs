using Catalog.Application.Commands;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalog.API;

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
        services.AddHealthChecks()
            .AddMongoDb(Configuration["DatabaseSetting:ConnectionString"], "Catalog Mongo Db Health Check", HealthStatus.Degraded);
        services.AddSwaggerGen(config => config.SwaggerDoc("v1", new OpenApiInfo() { Title = "Catalog API", Version = "v1" }));

        //Dipendincy Injection 
        services.AddAutoMapper(typeof(Startup));
        services.AddMediatR(typeof(CreateProductCommand).GetTypeInfo().Assembly);

        services.AddScoped<ICatalogContext, CatalogContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBrandRepository, ProductRepository>();
        services.AddScoped<ITypesRepository, ProductRepository>();

        //Adding Authentication Settings.

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:9009";
                options.Audience = "Catalog";
            });

        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        services.AddControllers(options =>
        {
            options.Filters.Add(new AuthorizeFilter(policy));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API"));
        }
        app.UseAuthentication();
        app.UseRouting();
        app.UseStaticFiles();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            }); ;
        });
    }
}
