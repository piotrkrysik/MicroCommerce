# MicroCommerce – Microservices Lab

Projekt edukacyjny demonstrujący architekturę mikroserwisową opartą o **.NET 8**, **React**, konteneryzację (**Docker**) oraz zasadę **Polyglot Persistence**. System wykorzystuje komunikację asynchroniczną i centralny Gateway do obsługi procesów e-commerce.

## 🏗 Architektura systemu

System składa się z niezależnych usług, które współpracują w ramach jednej sieci Dockerowej:

*   **Client (SPA)**: Nowoczesny frontend w **React**. Komunikuje się wyłącznie z bramą API.
*   **API Gateway (Ocelot)**: Centralny punkt wejścia (port 8010). Odpowiada za routing ruchu do mikroserwisów.
*   **Catalog.API**: Zarządza asortymentem produktów (**SQL Server**).
*   **Basket.API**: Obsługuje koszyki zakupowe i inicjuje proces zamówienia (**Redis**).
*   **Ordering.API**: Przetwarza zamówienia odebrane asynchronicznie (**SQL Server**).
*   **EventBus (RabbitMQ)**: Broker wiadomości zapewniający komunikację między usługami.

## 🚀 Jak uruchomić?

Całe środowisko (frontend, backend, bazy danych) wstaje „jednym kliknięciem”.

1.  Upewnij się, że masz zainstalowany **Docker Desktop**.
2.  W folderze głównym otwórz terminal i wpisz komendę:
    `docker-compose up -d --build`
3.  Docker automatycznie zbuduje obrazy (w tym obraz Reacta serwowany przez Nginx) i uruchomi kontenery.

## 🔗 Punkty dostępowe (Endpoints)

| Usługa | URL | Opis |
| :--- | :--- | :--- |
| **Frontend App** | [http://localhost:3000](http://localhost:3000) | Aplikacja React (SPA) |
| **API Gateway** | [http://localhost:8010](http://localhost:8010) | Główny punkt dostępu (Ocelot) |
| **RabbitMQ UI** | [http://localhost:15672](http://localhost:15672) | Panel brokera (guest/guest) |
| **Catalog API** | [http://localhost:8000/swagger](http://localhost:8000/swagger) | Dokumentacja Swagger (opcjonalnie) |

## 🛠 Kluczowe funkcjonalności

*   **Full-Stack Orchestration**: Zarządzanie cyklem życia frontendu i backendu za pomocą Docker Compose.
*   **Event-Driven Architecture**: Wykorzystanie **MassTransit** i **RabbitMQ** do obsługi zdarzeń.
*   **Polyglot Persistence**: Dobór baz danych pod konkretne wymagania (Redis dla koszyka, SQL Server dla zamówień).
*   **API Gateway Pattern**: Ukrycie złożoności mikroserwisów za jedną bramą, co rozwiązuje problemy z CORS.

## 📝 Flow składania zamówienia (E2E)

1.  **Katalog**: Użytkownik przegląda produkty pobierane z Catalog.API.
2.  **Koszyk**: Dodaje produkty do koszyka (dane zapisywane w Redis).
3.  **Checkout**: W widoku Checkout wysyła formularz – Basket.API publikuje wiadomość do RabbitMQ.
4.  **Zamówienie**: Ordering.API konsumuje wiadomość i zapisuje zamówienie w SQL Server.
5.  **Historia**: Użytkownik widzi status w zakładce "Moje Zamówienia" (dane pobierane przez Gateway).