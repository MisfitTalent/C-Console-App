# Submission 2 Checklist

This document maps the application to the second submission requirements. Submission 2 builds on the core console backend from Submission 1 by making the architecture more extensible and documenting the design patterns used.

## Submission 2 Focus

Submission 2 focuses on architectural improvement, design patterns, maintainability, validation, and clear explanation of design choices.

## Rubric Mapping

| Criteria | Evidence in project |
| --- | --- |
| Design Patterns | Factory Pattern through `IUserFactory` and `UserFactory`; Strategy Pattern through `IPaymentProcessor` and `WalletPaymentProcessor`; Repository Pattern through `IUserRepository`, `IProductRepository`, `IOrderRepository`, and `IPaymentRepository`; DTO/Mapper pattern through `StoredUser`, `StoredProduct`, `StoredOrder`, and `StoredPayment`. |
| Architecture Improvement | Console menus depend on service interfaces, services hold business workflows, domain objects protect business rules, persistence is abstracted behind repository interfaces, and role-based user construction is centralized in a factory. |
| Advanced Features | JSON persistence, order history, wallet payments, admin reporting, product reviews, low-stock detection, back navigation, flexible product selection by ID or name, and locale-tolerant money input support realistic backend behavior beyond the first core flow. |
| Code Maintainability | Classes remain focused by responsibility: domain models hold business state, services hold workflows, repositories handle persistence, factories handle object creation, and console classes handle user interaction. |
| Testing / Validation | `dotnet build` verifies compilation; scripted console testing covered customer and administrator flows; runtime validation is handled by domain/service guard clauses and menu-level exception handling. |
| Documentation | `README.md`, `SUBMISSION1.md`, and this file explain functionality, structure, design choices, validation, and evidence against the rubric. |

## Design Patterns Used

### Factory Pattern

`IUserFactory` and `UserFactory` centralize the creation of `Customer` and `Administrator` objects. This prevents role-selection logic from being duplicated in registration, seed data, and JSON loading.

Implementation evidence:

- `Services/Contracts/IUserFactory.cs`
- `Services/Implementations/UserFactory.cs`
- `Services/Implementations/UserService.cs`
- `Infrastructure/Persistence/StoredUser.cs`

Why it matters:

- Adding another role later only requires changing the factory instead of every place that creates users.
- User validation and wallet initialization stay consistent.
- `UserService` no longer needs to know how each concrete user subtype is constructed.

### Strategy Pattern

`IPaymentProcessor` defines the payment behavior used during checkout. `WalletPaymentProcessor` is the current implementation.

Implementation evidence:

- `Services/Contracts/IPaymentProcessor.cs`
- `Services/Implementations/WalletPaymentProcessor.cs`
- `Services/Implementations/OrderService.cs`

Why it matters:

- Checkout depends on the payment abstraction, not a hard-coded payment method.
- A future card, voucher, or EFT payment processor can be added without rewriting `OrderService`.
- The order workflow remains stable while payment behavior can vary.

### Repository Pattern

`IUserRepository`, `IProductRepository`, `IOrderRepository`, and `IPaymentRepository` define persistence operations. The current implementations are JSON-based repositories.

Implementation evidence:

- `Infrastructure/Persistence/IUserRepository.cs`
- `Infrastructure/Persistence/IProductRepository.cs`
- `Infrastructure/Persistence/IOrderRepository.cs`
- `Infrastructure/Persistence/IPaymentRepository.cs`
- `Infrastructure/Persistence/UserJsonStore.cs`
- `Infrastructure/Persistence/ProductJsonStore.cs`
- `Infrastructure/Persistence/OrderJsonStore.cs`
- `Infrastructure/Persistence/PaymentJsonStore.cs`

Why it matters:

- The application does not need to care whether data is stored in JSON, a database, or another persistence mechanism.
- Persistence responsibilities are separated from business services.
- The JSON implementation can be replaced later with less impact on the rest of the code.

### DTO / Mapper Pattern

`StoredUser`, `StoredProduct`, `StoredOrder`, and `StoredPayment` are persistence-friendly data shapes. They convert between runtime domain objects and JSON-friendly records.

Implementation evidence:

- `Infrastructure/Persistence/StoredUser.cs`
- `Infrastructure/Persistence/StoredProduct.cs`
- `Infrastructure/Persistence/StoredOrder.cs`
- `Infrastructure/Persistence/StoredPayment.cs`

Why it matters:

- Domain objects remain focused on business behavior.
- JSON serialization concerns stay in the infrastructure layer.
- Persisted data can evolve without forcing domain models to become storage DTOs.

## Architecture Improvement

The application is organized into clear layers:

| Layer | Responsibility | Evidence |
| --- | --- | --- |
| Console | Menu display, user input, and routing | `Console/ShoppingConsoleApplication.cs`, `Console/CustomerMenu.cs`, `Console/AdministratorMenu.cs`, `Console/ConsoleInput.cs` |
| Services | Business workflows such as registration, checkout, product management, and reporting | `Services/Implementations/UserService.cs`, `ProductService.cs`, `OrderService.cs`, `ReportService.cs` |
| Domain | Business entities and rules | `Domain/Users`, `Domain/Products`, `Domain/Cart`, `Domain/Orders`, `Domain/Payments`, `Domain/Reviews` |
| Infrastructure | Data store coordination and JSON persistence | `Infrastructure/AppDataStore.cs`, `Infrastructure/Persistence` |

This structure improves maintainability because menu changes, business workflow changes, domain rule changes, and persistence changes are isolated from each other.

## Advanced Features

- JSON persistence keeps users, products, orders, payments, and reviews available between sessions.
- Wallet checkout debits customer balances and records successful payments.
- Inventory control reduces stock during checkout and allows administrator restocking.
- Product reviews persist with product data and contribute to average product ratings.
- Sales reports use LINQ grouping and aggregation to summarize top products and category sales.
- Back navigation allows users to cancel prompts with `B` or `back`.
- Product selection accepts IDs or exact product names where products are listed.
- Money input accepts both comma and dot decimal formats for practical console use.

## Maintainability Choices

- Public contracts define important behavior through interfaces.
- Domain models encapsulate state with private setters and controlled methods.
- Validation happens close to the rule being protected.
- Console classes avoid direct persistence and delegate workflows to services.
- Persistence DTOs prevent JSON concerns from leaking into domain models.
- Public classes and methods include XML summary comments for readability.

## Validation Evidence

Run the following command before submission:

```powershell
dotnet build
```

Expected result:

```text
Build succeeded.
0 Warning(s)
0 Error(s)
```

Manual/scripted console validation covered:

- Customer registration and login.
- Product browsing and search.
- Adding products to cart by product name.
- Viewing, updating, and removing cart items.
- Adding wallet funds.
- Checkout and wallet debit.
- Viewing order history and tracking orders.
- Reviewing a product by name.
- Administrator login.
- Adding, updating, restocking, viewing, and deleting products.
- Viewing low-stock products.
- Viewing orders and updating order status.
- Generating sales reports.

Observed result:

```text
Customer flow completed without invalid/cancelled/error markers.
Administrator flow completed without invalid/cancelled/error markers.
dotnet build completed with 0 warnings and 0 errors.
```

## Notes

This submission intentionally keeps the console interface and JSON storage simple. The goal is to demonstrate clean backend design and design pattern usage without over-engineering the assignment into a full web API or database-backed system.
