FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Src/Migration/OnlineShop.Migrations/OnlineShop.Migrations.csproj", "OnlineShop.Migrations/"]
RUN dotnet restore "Src/Migration/OnlineShop.Migrations/OnlineShop.Migrations.csproj"
COPY . .
WORKDIR "/src/OnlineShop.Migrations"
RUN dotnet build "OnlineShop.Migrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.Migrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.Migrations.dll"]
