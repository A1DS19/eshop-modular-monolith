namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler(
    ILogger<ProductPriceChangedEventHandler> logger,
    IBus bus
) : INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(
        ProductPriceChangedEvent notification,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "Domain event {DomainEvent} has been handled",
            notification.GetType().Name
        );

        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.Product.Id,
            Name = notification.Product.Name,
            Categories = notification.Product.Categories,
            Description = notification.Product.Description,
            ImageFile = notification.Product.ImageFile,
            Price = notification.Product.Price, // Sets new price
        };

        await bus.Publish(integrationEvent, cancellationToken);
    }
}
