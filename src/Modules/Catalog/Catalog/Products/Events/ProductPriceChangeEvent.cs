namespace Catalog.Products.Events;

public record ProductPriceChangeEvent(Product Product) : IDomainEvent;
