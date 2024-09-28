using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Dishes.API.Extensions;

public static class PresentationServiceRegistrationExtensions
{
    public static WebApplicationBuilder AddPresentationServices(this WebApplicationBuilder builder)
    {
        // Configure API versioning and Swagger
        builder.Services
            .ConfigureApiVersioning()
            .ConfigureSwagger();

        // Configure environment settings
        builder
            .ConfigureEnvironmentSettings();

        return builder;
    }

    /// <summary>
    /// Configures API versioning for the application.
    /// </summary>
    /// <param name="services">The service collection to add the API versioning services to.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1.0);
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    /// <summary>
    /// Configures Swagger for the application, including dynamically generating documentation for all API versions.
    /// </summary>
    /// <param name="services">The service collection to add the Swagger services to.</param>
    /// <returns>The updated service collection.</returns>
    private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Retrieve all API versions using reflection
            var apiVersionDescriptionProvider = services
                .BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                c.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = $"Dishes API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                    Description = "API from Dishes",
                    Contact = new OpenApiContact() { Name = "DishesAPI", Email = "dishes.email.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org.licenses/MIT") },
                });
            }

            // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            // {
            //    Description = "Enter the JWT token like this: Bearer {your token}\n\n" +
            //                  "Example: Bearer eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4iLCJJc3N1ZXIiOiJJc3N1",
            //    Name = "Authorization",
            //    Scheme = "Bearer",
            //    BearerFormat = "JWT",
            //    In = ParameterLocation.Header,
            //    Type = SecuritySchemeType.ApiKey
            // });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Configures environment settings for the application.
    /// </summary>
    /// <param name="builder">The web application builder to configure.</param>
    /// <returns>The updated web application builder.</returns>
    private static WebApplicationBuilder ConfigureEnvironmentSettings(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        if (builder.Environment.IsDevelopment())
            builder.Configuration.AddUserSecrets<Program>();

        return builder;
    }
}