namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;

public record CreateBasketResult(Guid Id);

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("Username is required");
    }
}

public class CreateBasketHandler(IBasketRepository repository)
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(
        CreateBasketCommand command,
        CancellationToken cancellationToken
    )
    {
        await repository.GetBasket(
            userName: command.ShoppingCart.UserName,
            asNoTracking: false,
            throwIfNotFound: false,
            cancellationToken: cancellationToken
        );

        var newBasket = CreateNewBasket(command.ShoppingCart);

        await repository.CreateBasket(newBasket, cancellationToken);

        return new CreateBasketResult(newBasket.Id);
    }

    private ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCart)
    {
        var basket = ShoppingCart.Create(Guid.NewGuid(), shoppingCart.UserName);

        foreach (var item in shoppingCart.Items)
        {
            basket.AddItem(item.ProductId, item.Quantity, item.Color, item.Price, item.ProductName);
        }

        return basket;
    }
}
