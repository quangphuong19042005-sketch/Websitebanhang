## ADDED Requirements

### Requirement: Product Catalog Filtering
The system SHALL allow users to filter the product catalog by category, price range, color, and size.

#### Scenario: User filters by Category
- **WHEN** user selects a category (e.g., "Áo") from the filter sidebar and submits
- **THEN** system displays only products belonging to that category
- **AND** the URL reflects the filter (e.g., `?categoryId=1`)

#### Scenario: User filters by Price Range
- **WHEN** user inputs a minimum and maximum price and submits
- **THEN** system displays only products whose price falls within that range

#### Scenario: User sorts by Price
- **WHEN** user selects "Giá: Thấp đến Cao" from the sort dropdown
- **THEN** system displays the products ordered by price ascending

### Requirement: Catalog Pagination
The system SHOULD support pagination to ensure the page loads quickly when there are many products.

#### Scenario: Navigating pages
- **WHEN** user clicks on page 2
- **THEN** system displays the second set of products
- **AND** the URL reflects the page (e.g., `?page=2`)
