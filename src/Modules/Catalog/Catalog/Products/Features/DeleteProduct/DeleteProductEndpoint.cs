namespace Catalog.Products.Features.DeleteProduct;

// public record DeleteProductRequest(Guid Id);

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/products/{id}",
                async (Guid id, ISender sender) =>
                {
                    var response = await sender.Send(new DeleteProductCommand(id));
                    var result = response.Adapt<DeleteProductResponse>();

                    return Results.Ok(result);
                }
            )
            .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Deletes a product.")
            .WithDescription("Deletes a product in the catalog.");
    }
}
