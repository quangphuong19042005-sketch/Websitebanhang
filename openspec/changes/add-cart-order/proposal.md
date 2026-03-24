# Proposal: Add Session-Based Shopping Cart and Order Management

## Goal
Implement a robust shopping cart system using ASP.NET Core Sessions and an order management system to allow customers to purchase products.

## Context
The current application allows users to browse products and categories but lacks the ability to add items to a cart or place orders. Adding these features is essential for a functional e-commerce website.

## Proposed Solution
1.  **Session-Based Cart**: Use `Microsoft.AspNetCore.Http.ISession` to store cart items. The session cookie will be configured with a long `MaxAge` and marked as `IsEssential` to ensure the cart survives browser restarts for a better user experience.
2.  **Order Persistence**: Once a user checks out, the cart data will be persisted to the database in `Orders` and `OrderDetails` tables.
3.  **Identity Integration**: Orders will be linked to the registered `ApplicationUser`.

## Key Components (Updated)

### 1. Data Models
- `CartItem`: POJO for session storage.
- `Order`: Main order record.
- `OrderDetail`: Line items for each order.
- `OrderStatus`: Enum or predefined strings (e.g., Pending, Processing, Shipped, Completed, Cancelled).

### 2. Order Management (Admin-side)
- `Admin/Order/Index`: List all customer orders.
- `Admin/Order/Details`: Detailed view of a single order.
- `Admin/Order/UpdateStatus`: Action to change the order status.

### 3. Order History (User-side)
- `ShoppingCart/OrderHistory`: List of past orders for the logged-in user.

## Alternatives Considered
- **Database-backend Cart**: Could store cart items in the database. 
- *Pros*: Persistent across devices/logins. 
- *Cons*: More database writes, needs cleanup for expired carts. 
- *Decision*: Proceed with Session as requested by the user for simplicity and performance.

## Risks
- **Session Timeout**: If a user takes too long to checkout, their cart might clear.
- **Serialization**: Large carts could slightly impact performance during serialization.
- **Concurrency**: Minimal risk for single-user sessions.
