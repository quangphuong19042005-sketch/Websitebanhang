## Context
Currently, the `Websitebanhang` project does not have a clearly separated Home/Landing page and Product catalog page out-of-the-box in the navigation structure. The user wants these two concepts formally separated to improve the user experience.

## Goals / Non-Goals

**Goals:**
- Provide a dedicated Home page (`Views/Home/Index.cshtml`) with landing-page specific UI (e.g., banners, highlighted information).
- Provide a dedicated Product page (`Views/Product/Index.cshtml` or a new user-facing view) that serves strictly as the catalog.
- Update the main layout navigation (`Views/Shared/_Layout.cshtml`) to easily link to both pages.

**Non-Goals:**
- Completely redesigning the visual styling from scratch (unless requested by user).
- Creating new backend API features; this is strictly a routing and UI layout reorganization.

## Decisions

- **Navigation Links**: We will modify `_Layout.cshtml` to ensure there are distinct "Trang chủ" (Home) and "Sản phẩm" (Products) links. 
- **Controller Action**: We will ensure `HomeController.Index` returns a view that looks like a landing page (currently it returns a list of products, which might need to be stylized differently or limited to "featured" products). The `ProductController.Index` already returns a list of products, but typically this is used for an admin grid. We need to decide whether to reuse `ProductController.Index` for the user catalog or create a new user-facing action. We will assume for this change that we'll make the `Product/Index` the user-facing catalog and maintain its current functionality while cleaning up the look.

## Risks / Trade-offs
- **Admin vs Public View Risk**: If `ProductController.Index` was meant to be purely an Admin view (with Edit/Delete buttons), exposing it as the main public catalog might expose those buttons to regular users. 
  - *Mitigation*: Ensure the UI handles this or we create a dedicated public product listing action (e.g. `Product/Catalog`) if the current one has admin-only features. However, based on the codebase, we'll try to keep it simple and just organize what exists.
