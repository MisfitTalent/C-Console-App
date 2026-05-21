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

Submission 2 is supported through architecture and design improvements: domain models are separated from services and console menus, behavior is expressed through interfaces, and payment processing uses a replaceable strategy-style abstraction through `IPaymentProcessor`.

## Design Notes

The application separates domain models, service interfaces, service implementations, and console menus. Payment processing is abstracted behind `IPaymentProcessor`, allowing other payment approaches to be added without changing order placement logic.

Data is stored in memory because the specification asks for a console-based backend simulation using collections such as `List<T>`.

## Project Structure

- `Console/` contains menu flow, console input validation, and rendering helpers.
- `Domain/` contains business objects such as users, products, carts, orders, payments, reviews, and reports.
- `Infrastructure/` contains the in-memory data store and seed data.
- `Infrastructure/Persistence/` contains JSON persistence for users.
- `Services/Contracts/` contains service interfaces.
- `Services/Implementations/` contains business service implementations.

## Data Persistence

Registered users are saved to `Data/Users.json` so they can log in again after the console application is closed and restarted.

Products, orders, and payments are still stored in memory for the current application session.

## Run

```powershell
dotnet run
```
