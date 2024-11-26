using Catalog.Contracts.Products.Features.GetProductById;

namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.ShoppingCartItem.ShoppingCartId).NotNull();
        RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0);
    }
}

public class AddItemIntoBasketHandler(IBasketRepository repository, ISender sender)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(
        AddItemIntoBasketCommand command,
        CancellationToken cancellationToken
    )
    {
        var basket = await repository.GetBasket(command.UserName, true, true, cancellationToken);

        // sync call to catalog module to fetch product details
        var product = await sender.Send(
            new GetProductByIdQuery(command.ShoppingCartItem.ProductId),
            cancellationToken
        );

        basket.AddItem(
            command.ShoppingCartItem.ProductId,
            command.ShoppingCartItem.Quantity,
            command.ShoppingCartItem.Color,
            product.Product.Price,
            product.Product.Name
        // command.ShoppingCartItem.Price,
        // command.ShoppingCartItem.ProductName
        );

        await repository.SaveChangesAsync(command.UserName, cancellationToken: cancellationToken);

        return new AddItemIntoBasketResult(basket.Id);
    }
}
