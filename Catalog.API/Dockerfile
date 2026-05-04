FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Kopiujemy pliki projektów (.csproj) wszystkich warstw
COPY ["Catalog.API/Catalog.API.csproj", "Catalog.API/"]
COPY ["Catalog.Domain/Catalog.Domain.csproj", "Catalog.Domain/"]
COPY ["Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Catalog.Infrastructure/"]

# 2. Przywracamy paczki NuGet dla głównego projektu
RUN dotnet restore "Catalog.API/Catalog.API.csproj"

# 3. Kopiujemy całą resztę kodu źródłowego solucji
COPY . .

# 4. Ustawiamy się w folderze API i budujemy
WORKDIR "/src/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]