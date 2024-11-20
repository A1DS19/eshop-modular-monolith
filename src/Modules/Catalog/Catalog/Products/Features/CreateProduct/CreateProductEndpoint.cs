namespace Catalog.Products.Features.CreateProduct;

public record CreateProductRequest(ProductDto ProductDto);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/products",
                async (CreateProductRequest request, ISender sender) =>
                {
                    var command = request.ProductDto.Adapt<CreateProductCommand>();
                    var response = await sender.Send(command);
                    var result = response.Adapt<CreateProductResponse>();

                    return Results.Created($"/products/{result.Id}", result);
                }
            )
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Creates a new product.")
            .WithDescription("Creates a new product in the catalog.");
    }
}
