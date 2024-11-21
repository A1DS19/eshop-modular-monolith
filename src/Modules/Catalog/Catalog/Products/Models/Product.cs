namespace Catalog.Products.Models;

public class Product : Aggregate<Guid>
{
    public string Name { get; private set; } = default!;
    public List<string> Categories { get; private set; } = new();
    public string Description { get; private set; } = default!;
    public string ImageFile { get; private set; } = default!;
    public decimal Price { get; private set; }

    public static Product Create(
        Guid id,
        string name,
        List<string> categories,
        string description,
        string imageFile,
        decimal price
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

        var product = new Product
        {
            Id = id,
            Name = name,
            Categories = categories,
            Description = description,
            ImageFile = imageFile,
            Price = price,
        };

        product.AddDomainEvent(new ProductCreatedEvent(product));

        return product;
    }

    public static void Update(
        Product product,
        string name,
        List<string> categories,
        string description,
        string imageFile,
        decimal price
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

        product.Name = name;
        product.Categories = categories;
        product.Description = description;
        product.ImageFile = imageFile;
        product.Price = price;

        if (product.Price != price)
        {
            product.AddDomainEvent(new ProductPriceChangeEvent(product));
        }
    }
}
