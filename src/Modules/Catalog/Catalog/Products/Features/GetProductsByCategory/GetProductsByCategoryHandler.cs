namespace Catalog.Products.Features.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(List<ProductDto> Products);

public class GetProductByCategoryQueryValidator : AbstractValidator<GetProductsByCategoryQuery>
{
    public GetProductByCategoryQueryValidator()
    {
        RuleFor(x => x.Category).NotEmpty();
    }
}

public class GetProductsByCategoryHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(
        GetProductsByCategoryQuery query,
        CancellationToken cancellationToken
    )
    {
        var products = await dbContext
            .Products.AsNoTracking()
            .Where(p => p.Category.Contains(query.Category))
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(products.Adapt<List<ProductDto>>());
    }
}
