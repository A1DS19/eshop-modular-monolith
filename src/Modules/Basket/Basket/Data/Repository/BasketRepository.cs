namespace Basket.Data.Repository;

public class BasketRepository(BasketDbContext dbContext) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(
        string userName,
        bool asNoTracking = true,
        bool throwIfNotFound = true, // true when you want to throw an exception if the basket is not found, false when you want to throw an exception if the basket already exists
        CancellationToken cancellationToken = default
    )
    {
        var query = dbContext
            .ShoppingCarts.Include(x => x.Items)
            .Where(x => x.UserName == userName);

        if (asNoTracking)
        {
            query.AsNoTracking();
        }

        var basket = await query.SingleOrDefaultAsync(cancellationToken);

        if (basket is null && throwIfNotFound)
        {
            throw new BasketNotFoundException(userName);
        }

        if (basket is not null && !throwIfNotFound)
        {
            throw new BasketAlreadyExistsException(userName);
        }

        return basket;
    }

    public async Task<ShoppingCart> CreateBasket(
        ShoppingCart basket,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.ShoppingCarts.Add(basket);
        await dbContext.SaveChangesAsync(cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasket(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        var basket = await GetBasket(
            userName,
            asNoTracking: false,
            cancellationToken: cancellationToken
        );
        dbContext.ShoppingCarts.Remove(basket);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<int> SaveChangesAsync(
        string? userName = null,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
