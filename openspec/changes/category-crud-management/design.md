## Context
The application currently lacks an administrative interface for managing product categories. This design outlines the implementation of a full CRUD (Create, Read, Update, Delete) system for the `Category` model.

## Goals / Non-Goals

**Goals:**
- Provide a responsive management dashboard for categories (`Index` view).
- Implement forms for creating (`Add` view) and editing (`Update` view) categories.
- Enhance the repository layer to support all CRUD operations asynchronously.
- Ensure the user interface follows the modern, premium aesthetic established for product management.
- Support the `FolderName` property introduced recently to ensure correct image management.

**Non-Goals:**
- Bulk import/export of categories.
- Complex category hierarchies (sub-categories).
- Visual category grouping/nesting beyond simple list management.

## Decisions
- **Controller**: Create `CategoryController` with actions: `Index`, `Add` (GET/POST), `Update` (GET/POST), and `Delete` (POST).
- **Repository**: Update `ICategoryRepository` to include `Task AddAsync(Category category)`, `Task UpdateAsync(Category category)`, and `Task DeleteAsync(int id)`.
- **Views**: 
    - `Index.cshtml`: Table-based overview with "Edit" and "Delete" actions.
    - `Add.cshtml` & `Update.cshtml`: Clean forms with validation.
- **Styling**: Utilize the same premium dashboard design patterns used in the Product Management pages (shadows, rounded corners, clear typography).

## Risks / Trade-offs
- **Referential Integrity**: Deleting a category that has products.
    - *Mitigation*: Implement a check or handle the exception to prevent deleting categories that are currently in use, or use a "soft delete" if requested later. For now, simple error handling/prevention in the controller.
- **UI Consistency**: Ensuring the category pages match the product management's "premium" look.
    - *Mitigation*: Re-use CSS classes and layout structures from `Product/Index.cshtml`.
