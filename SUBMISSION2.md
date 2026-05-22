# Submission 2 Checklist

This document maps the application to the second submission requirements. Submission 2 builds on the core console backend from Submission 1 by making the architecture more extensible and documenting the design patterns used.

## Submission 2 Focus

Submission 2 focuses on architectural improvement, design patterns, maintainability, validation, and clear explanation of design choices.

## Rubric Mapping

| Criteria | Evidence in project |
| --- | --- |
| Design Patterns | Factory Pattern through `IUserFactory` and `UserFactory`; Strategy Pattern through `IPaymentProcessor` and `WalletPaymentProcessor`; Repository Pattern through `IUserRepository`, `IProductRepository`, `IOrderRepository`, and `IPaymentRepository`; DTO/Mapper pattern through `StoredUser`, `StoredProduct`, `StoredOrder`, and `StoredPayment`. |
| Architecture Improvement | Console menus depend on service interfaces, services depend on shared application data, persistence is abstracted behind repository interfaces, and role-based user construction is centralized in a factory. |
| Advanced Features | JSON persistence, order history, wallet payments, admin reporting, product reviews, low-stock detection, and back navigation support realistic backend behavior beyond the first core flow. |
| Code Maintainability | Classes remain focused by responsibility: domain models hold business state, services hold workflows, repositories handle persistence, and console classes handle user interaction. |
| Testing / Validation | `dotnet build` verifies compilation; runtime validation is handled by domain/service guard clauses and menu-level exception handling. |
| Documentation | `README.md`, `SUBMISSION1.md`, and this file explain functionality, structure, and design choices. |

## Design Patterns Used

### Factory Pattern

`IUserFactory` and `UserFactory` centralize the creation of `Customer` and `Administrator` objects. This prevents role-selection logic from being duplicated in registration, seed data, and JSON loading.

Why it matters:

- Adding another role later only requires changing the factory instead of every place that creates users.
- User validation and wallet initialization stay consistent.
- `UserService` no longer needs to know how each concrete user subtype is constructed.

### Strategy Pattern

`IPaymentProcessor` defines the payment behavior used during checkout. `WalletPaymentProcessor` is the current implementation.

Why it matters:

- Checkout depends on the payment abstraction, not a hard-coded payment method.
- A future card, voucher, or EFT payment processor can be added without rewriting `OrderService`.
- The order workflow remains stable while payment behavior can vary.

### Repository Pattern

`IUserRepository`, `IProductRepository`, `IOrderRepository`, and `IPaymentRepository` define persistence operations. The current implementations are JSON-based repositories.

Why it matters:

- The application does not need to care whether data is stored in JSON, a database, or another persistence mechanism.
- Persistence responsibilities are separated from business services.
- The JSON implementation can be replaced later with less impact on the rest of the code.

### DTO / Mapper Pattern

`StoredUser`, `StoredProduct`, `StoredOrder`, and `StoredPayment` are persistence-friendly data shapes. They convert between runtime domain objects and JSON-friendly records.

Why it matters:

- Domain objects remain focused on business behavior.
- JSON serialization concerns stay in the infrastructure layer.
- Persisted data can evolve without forcing domain models to become storage DTOs.

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

## Notes

This submission intentionally keeps the console interface and JSON storage simple. The goal is to demonstrate clean backend design and design pattern usage without over-engineering the assignment into a full web API or database-backed system.
