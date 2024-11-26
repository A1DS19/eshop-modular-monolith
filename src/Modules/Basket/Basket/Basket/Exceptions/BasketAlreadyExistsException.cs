namespace Basket.Basket.Exceptions;

public class BasketAlreadyExistsException : AlreadyExistsException
{
    public BasketAlreadyExistsException(string message)
        : base(message) { }

    public BasketAlreadyExistsException(string name, object key)
        : base(name, key) { }
}
