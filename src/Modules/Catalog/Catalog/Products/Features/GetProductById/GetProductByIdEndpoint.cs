using Catalog.Contracts.Products.Features.GetProductById;

namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/products/{id}",
                async (Guid id, ISender sender) =>
                {
                    var response = await sender.Send(new GetProductByIdQuery(id));
                    var result = response.Adapt<GetProductByIdResponse>();
                    return Results.Ok(result);
                }
            )
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Gets a product by its ID.")
            .WithDescription("Gets a product in the catalog by its ID.");
    }
}
