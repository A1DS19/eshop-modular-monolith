FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src

COPY ["src/Bootstrapper/API/API.csproj", "src/Bootstrapper/API/"]
COPY ["src/Modules/Basket/Basket/Basket.csproj", "src/Modules/Basket/Basket/"]
COPY ["src/Shared/Shared/Shared.csproj", "src/Shared/Shared/"]
COPY ["src/Shared/Shared.Contracts/Shared.Contracts.csproj", "src/Shared/Shared.Contracts/"]
COPY ["src/Modules/Catalog/Catalog/Catalog.csproj", "src/Modules/Catalog/Catalog/"]
COPY ["src/Modules/Catalog/Catalog.Contracts/Catalog.Contracts.csproj", "src/Modules/Catalog/Catalog.Contracts/"]
COPY ["src/Modules/Ordering/Ordering/Ordering.csproj", "src/Modules/Ordering/Ordering/"]

RUN dotnet restore "src/Bootstrapper/API/API.csproj"
COPY . .
WORKDIR "/src/src/Bootstrapper/API"
RUN dotnet build "API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
