namespace Catalog.Products.Features.GetProducts;

// public record GetProductsRequest(PaginatedRequest PaginationRequest);

public record GetProductsResponse(PaginatedResult<ProductDto> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products",
                async ([AsParameters] PaginatedRequest request, ISender sender) =>
                {
                    var response = await sender.Send(new GetProductsQuery(request));
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
