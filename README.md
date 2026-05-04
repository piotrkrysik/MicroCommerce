# MicroCommerce – Microservices Lab

Projekt edukacyjny demonstrujący architekturę mikroserwisową opartą o **.NET 8**, konteneryzację (**Docker**) oraz zasadę **Polyglot Persistence** (użycie różnych baz danych dla różnych potrzeb biznesowych).

## 🏗 Architektura systemu

System składa się z niezależnych usług, które komunikują się ze sobą wewnątrz sieci Dockerowej:

*   **Catalog.API**: Zarządza asortymentem produktów.
    *   **Technologia**: ASP.NET Core 8, Dapper/Entity Framework.
    *   **Baza danych**: SQL Server (dane strukturalne, relacyjne).
*   **Basket.API**: Obsługuje koszyki zakupowe użytkowników.
    *   **Technologia**: ASP.NET Core 8.
    *   **Baza danych**: Redis (szybka baza klucz-wartość w pamięci RAM).



## 🚀 Jak uruchomić?

Projekt jest w pełni scentralizowany dzięki **Docker Compose**. Nie musisz instalować SQL Servera ani Redisa lokalnie.

1.  Upewnij się, że masz zainstalowany **Docker Desktop**.
2.  Sklonuj repozytorium.
3.  W folderze głównym otwórz terminal i wpisz:
    ```bash
    docker-compose up -d
4. Wszystkie usługi zostaną zbudowane i uruchomione automatycznie.

## 🔗 Punkty dostępowe (Endpoints)

| Usługa | URL (Swagger) | Baza danych | Port |
| :--- | :--- | :--- | :--- |
| **Catalog API** | [http://localhost:8000/swagger](http://localhost:8000/swagger) | SQL Server | 8000 |
| **Basket API** | [http://localhost:8001/swagger](http://localhost:8001/swagger) | Redis | 8001 |

## 🛠 Kluczowe funkcjonalności

*   **Containerization**: Pełna izolacja środowiska za pomocą Docker.
*   **Data Persistence**: Dane koszyka są trwałe (nie znikają po restarcie kontenera Basket API).
*   **Clean Architecture**: Podział na warstwy API, Domain i Infrastructure w serwisie Katalogu.
*   **Redis Integration**: Wykorzystanie `IDistributedCache` do obsługi stanów koszyka.