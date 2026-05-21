# Submission 1 Checklist

This document maps the console application to the first submission requirements.

## Scope

Submission 1 focuses on the core backend functionality of the Online Shopping System. The application is implemented as a C# console app and stores data in memory using collections.

## Folder Structure

- `Console/`: numbered menus, input handling, and display rendering.
- `Domain/`: core entities and enums used by the shopping system.
- `Infrastructure/`: in-memory data storage, startup seed data, and JSON persistence.
- `Data/`: persisted user account data.
- `Services/Contracts/`: interfaces that describe application operations.
- `Services/Implementations/`: service classes that implement business logic.

## Rubric Mapping

| Criteria | Evidence in project |
| --- | --- |
| System Functionality | Registration, login, customer menu, administrator menu, product catalog, cart, checkout, wallet payments, orders, inventory, reviews, and reports are implemented. |
| Object-Oriented Design | Domain classes include `User`, `Customer`, `Administrator`, `Product`, `Cart`, `CartItem`, `Order`, `OrderItem`, `Payment`, and `Review`. Services and interfaces separate behavior from console input. |
| Console Interface | `ShoppingConsoleApplication`, `CustomerMenu`, and `AdministratorMenu` provide numbered menus for all required user actions. |
| LINQ Usage | Product search, product ordering, low-stock filtering, order history, and sales reports use LINQ queries. |
| Exception Handling | Services and domain objects validate input and throw clear exceptions; menu handlers catch validation errors and keep the console running. |
| Code Quality | Classes are focused, public members are documented, naming is clear, and generated build folders are excluded by `.gitignore`. |

## Required Features

- User registration and login
- Customer and administrator role separation
- Product catalog management
- Shopping cart functionality
- Order placement and tracking
- Payment simulation through wallet balance
- Registered user persistence through `Data/Users.json`
- Inventory management and stock tracking
- Administrative reporting and analytics
- Product review and rating system
- Exception handling and input validation

## Demo Accounts

- Administrator: `admin` / `admin123`
- Customer: `customer` / `customer123`

## Suggested Demo Flow

1. Run `dotnet run`.
2. Log in as `customer` / `customer123`.
3. Browse products.
4. Add a product to the cart.
5. View the cart.
6. Checkout.
7. View order history.
8. Log out.
9. Log in as `admin` / `admin123`.
10. View orders.
11. Update an order status.
12. Generate a sales report.

## Verification

Run:

```powershell
dotnet build
```

Expected result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```
