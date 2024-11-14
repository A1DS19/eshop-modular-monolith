#region Add services to the container.
var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

#endregion

#region Configure the HTTP request pipeline.
var app = builder.Build();

app.UseCatalogModule().UseBasketModule().UseOrderingModule();

#endregion

app.Run();
