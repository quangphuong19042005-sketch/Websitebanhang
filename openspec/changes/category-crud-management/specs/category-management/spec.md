## ADDED Requirements

### Requirement: CRUD Category Management
The system SHALL provide an interface for administrators to manage product categories.

#### Scenario: Admin views category list
- **WHEN** the admin navigates to the Category Management page (`/Category/Index`)
- **THEN** the system SHALL display a list of all existing categories, including their names and associated folder names.

#### Scenario: Admin adds a new category
- **WHEN** the admin fills out the "Add Category" form and submits
- **THEN** the system SHALL persist the new category to the database
- **AND** redirect the admin back to the Category Index with a success message.

#### Scenario: Admin updates an existing category
- **WHEN** the admin edits a category's details and submits
- **THEN** the system SHALL update the record in the database
- **AND** reflect the changes in the product catalog where applicable.

#### Scenario: Admin deletes a category
- **WHEN** the admin chooses to delete a category
- **THEN** the system SHALL prompt for confirmation
- **AND** upon confirmation, remove the category from the database (unless it has associated products).
