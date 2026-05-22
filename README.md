# Online Shopping Backend System

This is a C# console application that simulates the backend operations of a real-world online shopping platform while running entirely through a command-line interface.

The system allows customers to browse products, manage shopping carts, place orders, process payments through a simulated wallet, and track order history. Administrators can manage products, control inventory levels, process orders, and generate sales reports.

The project demonstrates object-oriented programming, LINQ-based querying, exception handling, role-based workflows, and clean system design in a structured console backend.

## Default Users

- Administrator: `admin` / `admin123`
- Customer: `customer` / `customer123`

## Implemented Features

- User registration and login
- Customer and administrator role separation
- Product catalog management
- Shopping cart management
- Wallet-based checkout
- Order placement and tracking
- Inventory and low-stock tracking
- Product reviews and ratings
- Administrative order viewing and status updates
- Sales reports using LINQ aggregation
- Input validation and exception handling

## Submission Coverage

Submission 1 is covered by the core backend functionality: authentication, role separation, product management, cart management, checkout, wallet payment, order tracking, inventory management, reporting, LINQ queries, validation, and a menu-driven console interface.

Submission 2 is supported through architecture and design improvements: domain models are separated from services and console menus, behavior is expressed through interfaces, user creation is centralized through a Factory pattern, persistence uses repository-style abstractions, and payment processing uses a replaceable Strategy pattern through `IPaymentProcessor`.

## Design Notes

The application separates domain models, service interfaces, service implementations, persistence repositories, and console menus. Payment processing is abstracted behind `IPaymentProcessor`, allowing other payment approaches to be added without changing order placement logic.

Data is stored in memory because the specification asks for a console-based backend simulation using collections such as `List<T>`.

Design pattern summary:

- Factory Pattern: `IUserFactory` and `UserFactory` create role-specific users.
- Strategy Pattern: `IPaymentProcessor` allows payment behavior to be replaced.
- Repository Pattern: persistence interfaces separate storage contracts from JSON implementations.
- DTO/Mapper Pattern: `Stored*` classes convert domain objects to JSON-friendly records.

## Project Structure

- `Console/` contains menu flow, console input validation, and rendering helpers.
- `Domain/` contains business objects such as users, products, carts, orders, payments, reviews, and reports.
- `Infrastructure/` contains the in-memory data store and seed data.
- `Infrastructure/Persistence/` contains repository interfaces and JSON persistence implementations.
- `Services/Contracts/` contains service interfaces.
- `Services/Implementations/` contains business service implementations.

## Data Persistence

Application data is saved in local JSON files so the system can resume state after the console application is closed and restarted.

- `Data/Users.json` stores users and wallet balances.
- `Data/Products.json` stores product catalog data, inventory, and reviews.
- `Data/Orders.json` stores customer orders linked by `CustomerId`.
- `Data/Payments.json` stores wallet payments linked by `OrderId` and `CustomerId`.

## Run

```powershell
dotnet run
```

## Verify

```powershell
dotnet build
```

Expected result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```
