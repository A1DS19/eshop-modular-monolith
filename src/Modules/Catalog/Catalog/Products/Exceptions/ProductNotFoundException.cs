namespace Catalog.Products.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(Guid productId)
        : base("Product", productId) { }
}
