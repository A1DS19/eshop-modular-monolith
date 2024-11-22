namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginatedRequest PaginationRequest) : IQuery<GetProductsResult>;

public record GetProductsResult(PaginatedResult<ProductDto> Products);

public class GetProductsValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsValidator()
    {
        RuleFor(x => x.PaginationRequest)
            .NotNull()
            .WithMessage("Pagination request cannot be null.");
        RuleFor(x => x.PaginationRequest.PageIndex)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page index must be greater than or equal to 1.");
        RuleFor(x => x.PaginationRequest.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be greater than or equal to 1.");
    }
}

public class GetProductsHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken
    )
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);

        var products = await dbContext
            .Products.AsNoTracking()
            .OrderBy(p => p.Name)
            .Paginate(query.PaginationRequest)
            .ToListAsync(cancellationToken);

        return new GetProductsResult(
            new PaginatedResult<ProductDto>(
                pageIndex,
                pageSize,
                totalCount,
                products.Select(p => p.Adapt<ProductDto>())
            )
        );
    }
}
