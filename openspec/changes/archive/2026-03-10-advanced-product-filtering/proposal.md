# Proposal: Advanced Product Catalog and Filtering

## Context
The current `Views/Home/Products.cshtml` page displays a simple, static list of products. To provide a realistic and engaging e-commerce experience, the user wants this page to feature advanced filtering (e.g., by category, price range, color, size), sorting, and potentially a more dynamic layout (like a sidebar for filters). This makes the store practical and user-friendly for customers browsing the catalog.

## What Changes
We will enhance the `Products.cshtml` page and its associated backend logic:
1.  **Sidebar Filtering UI**: Add a sidebar allowing users to select categories, price ranges, sizes, and colors.
2.  **Sorting & Pagination**: Add dropdowns to sort products (e.g., Price: Low to High, Newest) and support for pagination if there are many products.
3.  **Backend Support**: Update the `HomeController.Products` action to accept filter parameters (query strings) and filter the product list accordingly before passing it to the view.
4.  **Responsive Layout**: Ensure the new filtering system is mobile-friendly (perhaps a toggleable filter menu on smaller screens).

## Capabilities

### New Capabilities
- `catalog-filtering`: The ability for users to refine the product list by specific attributes (category, price, size, color) and sort the results.

### Modified Capabilities
- `<existing-name>`: N/A

## Impact
- `Controllers/HomeController.cs`: the `Products` action will need to handle query parameters.
- `Views/Home/Products.cshtml`: significant UI redesign to incorporate a sidebar, filter form, and improved grid.
- `Repositories/IProductRepository.cs` and `EFProductRepository.cs`: may need extending or modifying to support dynamic queries (e.g., IQueryable filtering) instead of just returning all products.
