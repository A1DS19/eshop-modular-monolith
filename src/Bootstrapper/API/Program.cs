using Basket;
using Catalog;
using Ordering;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

#region Add services to the container.

#endregion
var app = builder.Build();

#region Configure the HTTP request pipeline.

#endregion

app.Run();
