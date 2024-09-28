using Asp.Versioning.ApiExplorer;

namespace Dishes.API.Extensions;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add Services Layers
        builder
        .AddPresentationServices();
       
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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
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

            // Configure SwaggerUI to show all APIS versions
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services
                    .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"Dishes API {description.GroupName.ToUpperInvariant()}");
                }
            });
        }

        app.UseHttpsRedirection();
        return app;
    }
}