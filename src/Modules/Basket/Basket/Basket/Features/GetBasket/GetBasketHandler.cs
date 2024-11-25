namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);

public class GetBasketHandler(BasketDbContext dbContext)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(
        GetBasketQuery query,
        CancellationToken cancellationToken
    )
    {
        var basket = await dbContext
            .ShoppingCarts.AsNoTracking()
            .Include(e => e.Items)
            .SingleOrDefaultAsync(e => e.UserName == query.UserName, cancellationToken);

        if (basket is null)
        {
            throw new BasketNotFoundException(query.UserName);
        }

        return new GetBasketResult(basket.Adapt<ShoppingCartDto>());
    }
}
