namespace Basket.Basket.Exceptions;

public class BasketAlreadyExists : AlreadyExistsException
{
    public BasketAlreadyExists(string message)
        : base(message) { }

    public BasketAlreadyExists(string name, object key)
        : base(name, key) { }
}
