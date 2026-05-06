FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiujemy całą zawartość solucji do folderu /src w kontenerze
# To zagwarantuje, że wszystkie projekty i ich wzajemne powiązania będą zachowane
COPY . .

# Przywracamy paczki dla projektu Ordering (to automatycznie przywróci EventBus.Messages)
RUN dotnet restore "Ordering.API/Ordering.API.csproj"

# Budujemy projekt
WORKDIR "/src/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]