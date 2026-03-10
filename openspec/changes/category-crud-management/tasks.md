## 1. Repository Enhancements

- [ ] 1.1 Update `ICategoryRepository` interface with `AddAsync`, `UpdateAsync`, and `DeleteAsync` methods.
- [ ] 1.2 Implement these methods in `EFCategoryRepository` using Entity Framework Core.

## 2. Controller & Backend Implementation

- [ ] 2.1 Create `CategoryController.cs` with the following actions:
  - `Index`: Fetch and display all categories.
  - `Add` (GET): Display the creation form.
  - `Add` (POST): Handle form submission and persist a new category.
  - `Update` (GET): Fetch a category and display the edit form.
  - `Update` (POST): Handle form submission and update the existing category.
  - `Delete` (POST): Handle category deletion.

## 3. Frontend Implementation

- [ ] 3.1 Create `Views/Category/Index.cshtml` mirroring the premium dashboard layout of the Product Management page.
- [ ] 3.2 Create `Views/Category/Add.cshtml` with a modern form design.
- [ ] 3.3 Create `Views/Category/Update.cshtml` with a modern form design.
- [ ] 3.4 Update `Views/Shared/_Layout.cshtml` to include a "Quản lý danh mục" link in the "QUẢN TRỊ" dropdown menu.

## 4. Verification

- [ ] 4.1 Run `dotnet build` to verify no compilation errors.
- [ ] 4.2 Manually verify CRUD operations:
  - Check the list view.
  - Create a new category.
  - Edit an existing category.
  - Delete a category and ensure it works correctly (and handles associations safely).
