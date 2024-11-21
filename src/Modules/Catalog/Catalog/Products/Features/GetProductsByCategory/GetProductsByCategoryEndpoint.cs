namespace Catalog.Products.Features.GetProductsByCategory;

public record GetProductsByCategoryResponse(List<ProductDto> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products/category/{category}",
                async (string category, ISender sender) =>
                {
                    var response = await sender.Send(new GetProductsByCategoryQuery(category));
                    var result = response.Adapt<GetProductsByCategoryResponse>();

                    return Results.Ok(result);
                }
            )
            .WithName("GetProductsByCategory")
            .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Gets all products by category.")
            .WithDescription("Gets all products in the catalog by category.");
    }
}
