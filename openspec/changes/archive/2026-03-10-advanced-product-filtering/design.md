## Context
The user wants `Views/Home/Products.cshtml` to feel like a real e-commerce catalog page. Currently, it simply lists all products. To make it realistic, it needs filtering (Category, Price, Size, Color), sorting (e.g., Price: Low to High), and pagination.

## Goals / Non-Goals

**Goals:**
- Provide a responsive sidebar or top-bar UI for filtering products.
- Allow users to filter by `CategoryId`, `Price` ranges, `Size`, and `Color`.
- Allow sorting of results.
- Process these filters on the backend (`HomeController.Products`) by passing query parameters.
- Provide a `ProductFilterViewModel` or similar structure if necessary to pass filter data (like categories for the dropdowns/checkboxes) back to the view.

**Non-Goals:**
- Build a full Single Page Application (SPA) using React/Vue. We will stick to standard ASP.NET Core MVC server-side rendering with standard form submissions (GET requests) for filtering, perhaps enhanced with basic JavaScript.
- Complex full-text search engine integration (like ElasticSearch). Basic EF Core LINQ filtering is sufficient.

## Decisions
- **Form Method**: The filter form will use `method="get"` pointing to `HomeController.Products`. This ensures the URL updates with query parameters (e.g., `?categoryId=1&minPrice=100000`), allowing users to share or bookmark filtered links.
- **Repository Changes**: `IProductRepository` currently has `Task<IEnumerable<Product>> GetAllAsync();`. We will add a new method `Task<IEnumerable<Product>> GetFilteredAsync(int? categoryId, decimal? minPrice, decimal? maxPrice, string? size, string? color, string? sortOrder);` to handle the dynamic query building, or simply update `GetAllAsync` if the scope is small. For maintainability, creating a `GetFilteredAsync` or altering the existing one to accept a filter object is best.
- **ViewModel**: Create a `CatalogViewModel` containing `IEnumerable<Product> Products`, `SelectList Categories`, and properties for the current filter state so the UI can reflect the active filters.

## Risks / Trade-offs
- **Performance**: Fetching all records and then filtering in-memory (`IEnumerable`) is bad if the database grows.
  - *Mitigation*: The new repository method must build an `IQueryable<Product>` and apply `Where()` clauses *before* calling `.ToListAsync()` to ensure SQL Server does the filtering.
- **UI Complexity**: Creating a good looking filter sidebar can be tricky with vanilla Bootstrap.
  - *Mitigation*: We will use standard Bootstrap 5 components (offcanvas/collapsible accordions) to keep it clean and mobile-friendly.
