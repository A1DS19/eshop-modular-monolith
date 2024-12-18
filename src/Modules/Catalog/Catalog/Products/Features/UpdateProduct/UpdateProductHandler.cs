namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Product.Id).NotEmpty();
        RuleFor(x => x.Product.Name).NotEmpty();
        RuleFor(x => x.Product.Categories).NotEmpty();
        RuleFor(x => x.Product.Description).NotEmpty();
        RuleFor(x => x.Product.ImageFile).NotEmpty();
        RuleFor(x => x.Product.Price).GreaterThan(0);
    }
}

public class UpdateProductCommandHandler(CatalogDbContext dbContext)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(
        UpdateProductCommand command,
        CancellationToken cancellationToken
    )
    {
        var product = await dbContext.Products.FindAsync([command.Product.Id], cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Product.Id);
        }

        product.Update(
            command.Product.Name,
            command.Product.Categories,
            command.Product.Description,
            command.Product.ImageFile,
            command.Product.Price
        );

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}
