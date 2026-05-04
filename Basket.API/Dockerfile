FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# W .NET 8 domyślnym portem jest 8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiujemy plik projektu Basket.API (ścieżka względem kontekstu budowania, czyli głównego folderu)
COPY ["Basket.API/Basket.API.csproj", "Basket.API/"]

# Przywracamy zależności
RUN dotnet restore "Basket.API/Basket.API.csproj"

# Kopiujemy całą resztę plików
COPY . .

# Przechodzimy do folderu projektu Basket.API
WORKDIR "/src/Basket.API"

# Budujemy projekt
RUN dotnet build "Basket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]