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

public class CreateBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(
        CreateBasketCommand command,
        CancellationToken cancellationToken
    )
    {
        var basket = await dbContext
            .ShoppingCarts.AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.UserName == command.ShoppingCart.UserName,
                cancellationToken
            );

        if (basket is not null)
        {
            throw new BasketAlreadyExists("ShoppingCart", command.ShoppingCart.UserName);
        }

        var newBasket = CreateNewBasket(command.ShoppingCart);

        dbContext.ShoppingCarts.Add(newBasket);
        await dbContext.SaveChangesAsync(cancellationToken);

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
