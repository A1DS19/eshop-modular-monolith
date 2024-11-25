namespace Basket.Basket.Exceptions;

public class BasketNotFoundException : NotFoundException
{
    public BasketNotFoundException(string entity)
        : base($"ShoppingCart", entity) { }
}
