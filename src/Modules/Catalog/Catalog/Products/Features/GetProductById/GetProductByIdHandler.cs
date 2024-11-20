namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(ProductDto Product);

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(
        GetProductByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var product = await dbContext
            .Products.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (product is null)
        {
            throw new Exception($"Product with ID {query.Id} not found");
        }

        return new GetProductByIdResult(product.Adapt<ProductDto>());
    }
}
