using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Data.Repository;

public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache)
    : IBasketRepository
{
    private const string BasketCacheKeyPrefix = "Basket_";

    private string CreateBasketCacheKey(string userName) => BasketCacheKeyPrefix + userName;

    public async Task<ShoppingCart> GetBasket(
        string userName,
        bool asNoTracking = true,
        bool throwIfNotFound = true,
        CancellationToken cancellationToken = default
    )
    {
        var cachedKey = CreateBasketCacheKey(userName);

        if (!asNoTracking)
        {
            return await basketRepository.GetBasket(
                userName,
                asNoTracking,
                throwIfNotFound,
                cancellationToken
            );
        }

        var cachedBasket = await cache.GetStringAsync(cachedKey, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        var basket = await basketRepository.GetBasket(
            userName,
            asNoTracking,
            throwIfNotFound,
            cancellationToken
        );

        await cache.SetStringAsync(
            cachedKey,
            JsonSerializer.Serialize(basket),
            // new DistributedCacheEntryOptions
            // {
            //     AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            // },
            cancellationToken
        );

        return basket;
    }

    public async Task<ShoppingCart> CreateBasket(
        ShoppingCart basket,
        CancellationToken cancellationToken = default
    )
    {
        var cachedKey = CreateBasketCacheKey(basket.UserName);
        await basketRepository.CreateBasket(basket, cancellationToken);
        await cache.SetStringAsync(cachedKey, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<bool> DeleteBasket(
        string userName,
        CancellationToken cancellationToken = default
    )
    {
        var cachedKey = CreateBasketCacheKey(userName);
        await basketRepository.DeleteBasket(userName, cancellationToken);
        await cache.RemoveAsync(cachedKey, cancellationToken);
        return true;
    }

    public async Task<int> SaveChangesAsync(
        string? userName = null,
        CancellationToken cancellationToken = default
    )
    {
        var result = await basketRepository.SaveChangesAsync(userName, cancellationToken);

        if (userName is not null)
        {
            await cache.RemoveAsync(CreateBasketCacheKey(userName), cancellationToken);
        }

        return result;
    }
}
