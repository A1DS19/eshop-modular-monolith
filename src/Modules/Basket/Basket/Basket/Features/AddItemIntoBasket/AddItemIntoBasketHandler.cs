namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid id);

public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.ShoppingCartItem.ShoppingCartId).NotNull();
        RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0);
    }
}

public class AddItemIntoBasketHandler(IBasketRepository repository)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(
        AddItemIntoBasketCommand command,
        CancellationToken cancellationToken
    )
    {
        var basket = await repository.GetBasket(command.UserName, true, true, cancellationToken);

        AddItemsIntoBasket(basket, command.ShoppingCartItem);

        await repository.SaveChangesAsync(command.UserName, cancellationToken: cancellationToken);

        return new AddItemIntoBasketResult(basket.Id);
    }

    private void AddItemsIntoBasket(ShoppingCart basket, ShoppingCartItemDto item)
    {
        basket.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName);
    }
}
