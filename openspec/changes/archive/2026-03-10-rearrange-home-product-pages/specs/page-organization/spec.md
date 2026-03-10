## ADDED Requirements

### Requirement: Separate Home and Product Catalog
The system SHALL provide distinct web pages for the Home landing experience and the Product catalog browsing experience.

#### Scenario: User navigates to Home
- **WHEN** user clicks on the website logo or "Trang chủ" link (URL `/` or `/Home/Index`)
- **THEN** system displays the Home page with landing content (banners, featured info, introduction).
- **AND** system does not immediately show the full product catalog grid.

#### Scenario: User navigates to Products
- **WHEN** user clicks on the "Sản phẩm" link (URL `/Product` or `/Product/Index`)
- **THEN** system displays the full Product catalog.
- **AND** the page focuses solely on product browsing (filtering, listing).
