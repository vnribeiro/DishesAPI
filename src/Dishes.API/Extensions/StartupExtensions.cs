using Asp.Versioning;
using Dishes.API.Endpoints;

namespace Dishes.API.Extensions;

public static class StartupExtensions
{
    /// <summary>
    /// Configures services for the application.
    /// Adds persistence services, CORS policy, API versioning, Swagger, and environment settings.
    /// </summary>
    /// <param name="builder">The WebApplicationBuilder instance.</param>
    /// <returns>The configured WebApplication instance.</returns>
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

    /// <summary>
    /// Configures the HTTP request pipeline for the application.
    /// Enables CORS, Swagger, and maps endpoints.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <returns>The configured WebApplication instance.</returns>
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
            });
        }

        // Map the endpoints
        app.MapEndpoints();
        app.UseHttpsRedirection();
        return app;
    }
}