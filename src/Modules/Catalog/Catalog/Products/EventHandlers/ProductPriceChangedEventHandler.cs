namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger)
    : INotificationHandler<ProductPriceChangeEvent>
{
    public Task Handle(ProductPriceChangeEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain event {DomainEvent} has been handled",
            notification.GetType().Name
        );

        return Task.CompletedTask;
    }
}
