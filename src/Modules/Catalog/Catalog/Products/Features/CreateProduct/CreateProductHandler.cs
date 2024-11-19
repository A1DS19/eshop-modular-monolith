namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price
) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandHandler(CatalogDbContext dbContext)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
