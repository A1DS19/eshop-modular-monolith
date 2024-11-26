namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
    : ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(Guid Id);

public class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
    }
}

public class RemoveItemFromBasketHandler(IBasketRepository repository)
    : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(
        RemoveItemFromBasketCommand command,
        CancellationToken cancellationToken
    )
    {
        var basket = await repository.GetBasket(command.UserName, true, true, cancellationToken);

        RemoveItemFromBasket(basket, command.ProductId);

        await repository.SaveChangesAsync(cancellationToken: cancellationToken);

        return new RemoveItemFromBasketResult(basket.Id);
    }

    private void RemoveItemFromBasket(ShoppingCart basket, Guid productId)
    {
        basket.RemoveItem(productId);
    }
}
