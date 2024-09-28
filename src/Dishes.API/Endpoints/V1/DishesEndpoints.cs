namespace Dishes.API.Endpoints.V1;

public static class DishesEndpoints
{
    public static IEndpointRouteBuilder MapDishesEndpoints(this IEndpointRouteBuilder app)
    {
    
        app.MapGet("dishes", async (CancellationToken cancellationToken) =>
        {
            return Results.Ok(new { Message = "Products from V1" });
        });

        app.MapGet("dishes/{dishId:Guid}", async (Guid dishId, CancellationToken cancellationToken) =>
        {
            return Results.Ok(new { Message = $"Product from V1 with id {dishId}" });
        });

        return app;
    }
}