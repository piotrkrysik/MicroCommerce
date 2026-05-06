# MicroCommerce – Microservices Lab

Projekt edukacyjny demonstrujący architekturę mikroserwisową opartą o **.NET 8**, konteneryzację (**Docker**) oraz zasadę **Polyglot Persistence**. System wykorzystuje komunikację asynchroniczną do obsługi procesu składania zamówień.

## 🏗 Architektura systemu

System składa się z niezależnych usług komunikujących się asynchronicznie za pomocą brokera wiadomości:

*   **Catalog.API**: Zarządza asortymentem produktów. (SQL Server)
*   **Basket.API**: Obsługuje koszyki zakupowe i inicjuje proces zamówienia (Checkout). (Redis)
*   **Ordering.API**: Przetwarza zamówienia odebrane z kolejki. (SQL Server + MediatR/Uproszczony Controller)
*   **EventBus (RabbitMQ)**: Broker wiadomości zapewniający asynchroniczną komunikację między Basket a Ordering.

## 🚀 Jak uruchomić?

Projekt jest w pełni scentralizowany dzięki **Docker Compose**.

1.  Upewnij się, że masz zainstalowany **Docker Desktop**.
2.  Sklonuj repozytorium.
3.  W folderze głównym otwórz terminal i wpisz:
    ```bash
    docker-compose up -d --build
    ```
4.  Wszystkie usługi, bazy danych oraz broker RabbitMQ zostaną uruchomione automatycznie.

## 🔗 Punkty dostępowe (Endpoints)

| Usługa | URL (Swagger / Panel) | Baza danych | Port |
| :--- | :--- | :--- | :--- |
| **Catalog API** | [http://localhost:8000/swagger](http://localhost:8000/swagger) | SQL Server | 8000 |
| **Basket API** | [http://localhost:8001/swagger](http://localhost:8001/swagger) | Redis | 8001 |
| **Ordering API** | [http://localhost:8002/swagger](http://localhost:8002/swagger) | SQL Server | 8002 |
| **RabbitMQ Dashboard** | [http://localhost:15672](http://localhost:15672) (guest/guest) | - | 15672 |

## 🛠 Kluczowe funkcjonalności

*   **Event-Driven Architecture**: Wykorzystanie **MassTransit** i **RabbitMQ** do przesyłania zdarzeń (`BasketCheckoutEvent`) między mikroserwisami.
*   **Polyglot Persistence**: 
    *   **SQL Server** dla danych strukturalnych (Katalog, Zamówienia).
    *   **Redis** dla danych ulotnych/szybkich (Koszyk).
*   **Data Integrity**: Rozwiązanie problemów z mapowaniem danych płatności (CVV) oraz obsługa błędów zapisu w bazie danych.
*   **Containerization**: Pełna orkiestracja za pomocą Docker Compose.
*   **Observability**: Logowanie zdarzeń w konsoli kontenerów ułatwiające debugowanie przepływu wiadomości.

## 📝 Flow składania zamówienia
1. Dodaj produkty do koszyka w **Basket API**.
2. Wykonaj endpoint `/Checkout` w Basket API, podając dane użytkownika i karty.
3. Zdarzenie trafia do **RabbitMQ**.
4. **Ordering API** konsumuje wiadomość i trwale zapisuje zamówienie w bazie SQL Server.
5. Pobierz zamówienie przez endpoint `GET /api/v1/Order/{userName}` w Ordering API.