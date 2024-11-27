#region Add services to the container.
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, cfg) => cfg.ReadFrom.Configuration(context.Configuration));

var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;
var orderingAssembly = typeof(OrderingModule).Assembly;

builder.Services.AddCarterWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);
builder.Services.AddMediatRWithAssemblies(catalogAssembly, basketAssembly, orderingAssembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMassTransitAssemblies(catalogAssembly, basketAssembly);

builder
    .Services.AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

#endregion

#region Configure the HTTP request pipeline.
var app = builder.Build();

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(opts => { });

app.UseCatalogModule().UseBasketModule().UseOrderingModule();

app.Run();

#endregion
