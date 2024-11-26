using Catalog.Contracts.Products.Features.GetProductById;

namespace Catalog.Products.Features.GetProductById;

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
            throw new ProductNotFoundException(query.Id);
        }

        return new GetProductByIdResult(product.Adapt<ProductDto>());
    }
}
