using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Dishes.API.Endpoints;

namespace Dishes.API.Extensions;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {       
        builder.Services
        .AddPersistenceServices(builder.Configuration);
  
        // Add CORS
        builder.Services
            .AddCors(options =>
                options.AddPolicy("open", policy =>
                    policy.WithOrigins([
                            builder.Configuration["ApiUrl"] ??
                            "https://localhost:7020"
                        ])
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(pol => true)
                        .AllowAnyHeader()
                        .AllowCredentials()));

        builder.Services
        .AddEndpointsApiExplorer()
        .ConfigureApiVersioning()
        .ConfigureSwagger();

        builder
        .ConfigureEnvironmentSettings();
        
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Enable CORS
        app.UseCors("open");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            // Configure SwaggerUI to show all API versions
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dishes API V1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Dishes API V2");
            });         
        }

        // Map the endpoints
        app.MapEndpoints();
        app.UseHttpsRedirection();
        return app;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // Define the API version
        var apiVersionV1 = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1.0))
        .Build();

        var apiVersionV2 = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(2.0))
        .Build();

        // Map the API version to the endpoints
        var versionedGroupV1 = app
        .MapGroup("/api/v{apiVersion:apiVersion}")
        .WithApiVersionSet(apiVersionV1);
        
        var versionedGroupV2 = app
        .MapGroup("/api/v{apiVersion:apiVersion}")
        .WithApiVersionSet(apiVersionV2);
        
        // Map the endpoints V1
        versionedGroupV1
        .MapDishesEndpoints();

        versionedGroupV2
        .MapDishesEndpointsV2();

        return app;
    }
}