namespace Catalog.Products.Features.GetProducts;

public record GetProductsResponse(IEnumerable<ProductDto> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products",
                async (ISender sender) =>
                {
                    var query = new GetProductsQuery();
                    var response = await sender.Send(query);
                    var result = response.Adapt<GetProductsResponse>();

                    return Results.Ok(result);
                }
            )
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Gets all products.")
            .WithDescription("Gets all products in the catalog.");
    }
}
