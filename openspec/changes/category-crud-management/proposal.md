# Proposal: Category CRUD Management

## Context
Currently, the application has a `Category` model, `ICategoryRepository`, and `EFCategoryRepository` to fetch categories for products. However, there is no interface for an administrator to manage these categories (Create, Read, Update, Delete). To provide a fully functional administration area, the user needs a dedicated set of pages to manage categories.

## What Changes
We will create a full CRUD (Create, Read, Update, Delete) suite for Categories:
1.  **CategoryController**: A new controller to handle category-related actions.
2.  **Views**: Create `Index`, `Add`, and `Update` views under `Views/Category`.
3.  **Repository Enhancement**: Expand `ICategoryRepository` and `EFCategoryRepository` to include `AddAsync`, `UpdateAsync`, and `DeleteAsync` methods.
4.  **Navigation**: Add a "Quản lý danh mục" link under the "QUẢN TRỊ" dropdown menu in the `_Layout.cshtml` shared view.

## Capabilities

### New Capabilities
- `category-management`: The ability for administrators to view, add, edit, and delete product categories.

### Modified Capabilities
- `<existing-name>`: N/A

## Impact
- `Repositories/ICategoryRepository.cs` and `EFCategoryRepository.cs` will be extended.
- `Controllers/CategoryController.cs` will be created.
- `Views/Category/*` will be created.
- `Views/Shared/_Layout.cshtml` will be modified to add the navigation link.
