FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Src/Api/OnlineShop.RestApi/OnlineShop.RestApi.csproj", "OnlineShop.RestApi/"]
RUN dotnet restore "Src/Api/OnlineShop.RestApi/OnlineShop.RestApi.csproj"
COPY . .
WORKDIR "/src/OnlineShop.RestApi"
RUN dotnet build "OnlineShop.RestApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineShop.RestApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineShop.RestApi.dll"]
