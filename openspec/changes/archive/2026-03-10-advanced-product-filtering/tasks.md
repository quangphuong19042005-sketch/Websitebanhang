## 1. Backend Updates

- [x] 1.1 Create `CatalogViewModel` containing `IEnumerable<Product> Products`, `SelectList Categories`, and filter property fields.
- [x] 1.2 Update `IProductRepository` to include a new filtering method or modify `GetAllAsync` to accept filter arguments (`categoryId`, `minPrice`, `maxPrice`, `size`, `color`, `sortOrder`).
- [x] 1.3 Implement the new filtering logic in `EFProductRepository` using `IQueryable` for database-level filtering.
- [x] 1.4 Update `HomeController.Products` to accept query parameters mapping to the filter arguments, call the repository, construct `CatalogViewModel`, and return it.

## 2. Frontend Updates

- [x] 2.1 Refactor `Views/Home/Products.cshtml` to strongly type against `CatalogViewModel`.
- [x] 2.2 Add a responsive filter sidebar (Bootstrap Offcanvas or standard grid) with a `<form method="get">` to submit filters.
- [x] 2.3 Add filter inputs (Category dropdown, Price min/max inputs, Size/Color text or dropdowns, Sort by dropdown).
- [x] 2.4 Update the product grid to display the filtered results elegantly.
- [x] 2.5 Ensure pagination or "Load More" works if implemented (optional but recommended depending on product count).

## 3. Verification
- [x] 3.1 Run `dotnet build` to ensure no errors.
- [x] 3.2 Open the browser and test passing different filters in the UI. Ensure the URL updates correctly and the product grid reflects the active filters.
