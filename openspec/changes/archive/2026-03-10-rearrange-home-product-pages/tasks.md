## 1. Controller Updates

- [x] 1.1 Update `HomeController.Index` to return a dedicated Home/Landing View (or maintain current logic if it serves right, but ensure it differs from `ProductController.Index`).
- [x] 1.2 Verify `ProductController.Index` correctly serves the full product catalog.

## 2. View Updates

- [x] 2.1 Refactor/Create `Views/Home/Index.cshtml` to include landing page content (banners, welcome text, featured products).
- [x] 2.2 Refactor `Views/Product/Index.cshtml` (if necessary) to ensure it serves clearly as the product listing page.
- [x] 2.3 Update `Views/Shared/_Layout.cshtml` navigation menu to point "Trang chủ" to `/Home/Index` and "Sản phẩm" to `/Product/Index`.

## 3. Verification
- [ ] 3.1 Run `dotnet build` to verify no compilation errors.
- [ ] 3.2 Launch browser and verify Home page and Product page show expected distinct content.
