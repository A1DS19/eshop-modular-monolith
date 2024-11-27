namespace Basket.Basket.Exceptions;

public class ShoppingCartItemNotFoundException : NotFoundException
{
    public ShoppingCartItemNotFoundException(string entity)
        : base("ShoppingCartItem", entity) { }
}
