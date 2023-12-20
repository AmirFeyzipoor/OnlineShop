FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Src/Migration/OnlineShop.ReadableDataMigrations/OnlineShop.ReadableDataMigrations.csproj", "OnlineShop.ReadableDataMigrations/"]
RUN dotnet restore "Src/Migration/OnlineShop.ReadableDataMigrations/OnlineShop.ReadableDataMigrations.csproj"
COPY . .
WORKDIR "/src/OnlineShop.ReadableDataMigrations"
RUN dotnet build "OnlineShop.ReadableDataMigrations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.ReadableDataMigrations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.ReadableDataMigrations.dll"]
