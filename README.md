MicroCommerce – Microservices Lab
Projekt edukacyjny demonstrujący architekturę mikroserwisową opartą o .NET 8, konteneryzację (Docker) oraz zasadę Polyglot Persistence (użycie różnych baz danych dla różnych potrzeb biznesowych).
🏗 Architektura systemu
System składa się z niezależnych usług, które komunikują się ze sobą wewnątrz sieci Dockerowej:

Catalog.API: Zarządza asortymentem produktów.

Technologia: ASP.NET Core 8, Dapper/Entity Framework.

Baza danych: SQL Server (dane strukturalne, relacyjne).

Basket.API: Obsługuje koszyki zakupowe użytkowników.

Technologia: ASP.NET Core 8.

Baza danych: Redis (szybka baza klucz-wartość w pamięci RAM).
🚀 Jak uruchomić?
Projekt jest w pełni scentralizowany dzięki Docker Compose. Nie musisz instalować SQL Servera ani Redisa lokalnie.

Upewnij się, że masz zainstalowany Docker Desktop.

Sklonuj repozytorium.

W folderze głównym otwórz terminal i wpisz:
docker-compose up -d
Wszystkie usługi zostaną zbudowane i uruchomione automatycznie.
🔗 Punkty dostępowe (Endpoints)
UsługaURL (Swagger)Baza danychPort
Catalog APIhttp://localhost:8000/swaggerSQL Server8000Basket APIhttp://localhost:8001/swaggerRedis8001
