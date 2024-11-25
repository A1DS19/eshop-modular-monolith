namespace Basket.Basket.Features.DeleteBasket;

// public record DeleteBasketRequest(string Username);

public record DeleteBasketResponse(bool IsDeleted);

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
                "/basket/{userName}",
                async (string userName, ISender sender) =>
                {
                    var result = await sender.Send(new DeleteBasketCommand(userName));
                    var response = result.Adapt<DeleteBasketResponse>();
                    return Results.Ok(response);
                }
            )
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete a new basket.")
            .WithDescription("Delete a new basket.");
    }
}