FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Kopiujemy plik projektu API Gateway
COPY ["ApiGateway/ApiGateway.csproj", "ApiGateway/"]

# 2. Przywracamy paczki NuGet (m.in. Ocelota)
RUN dotnet restore "ApiGateway/ApiGateway.csproj"

# 3. Kopiujemy całą resztę kodu źródłowego
COPY . .

# 4. Budujemy projekt Gatewaya
WORKDIR "/src/ApiGateway"
RUN dotnet build "ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]