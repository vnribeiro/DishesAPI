using Asp.Versioning;

namespace Dishes.API.Endpoints;

public static class EndpointsMapping
{
    /// <summary>
    /// Maps API endpoints to specific API versions.
    /// Defines an API version set and maps the endpoints for version 1.0.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <returns>The configured WebApplication instance.</returns>
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // Define the API version
        var apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1.0))
        .Build();

        // Map the API version to the endpoints
        var versionedGroup = app
        .MapGroup("/api/v{apiVersion:apiVersion}")
        .WithApiVersionSet(apiVersionSet);

        // Map the endpoints V1
        versionedGroup
        .MapDishesEndpoints();

        return app;
    }
}