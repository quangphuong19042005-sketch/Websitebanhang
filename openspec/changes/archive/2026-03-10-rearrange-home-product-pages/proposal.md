# Proposal: Rearrange Home and Product Pages

## Context
The user wants to rearrange the website interface so that the "Home" page and the "Product" page are clearly separated and organized according to their intended purposes. Currently, the Home page might be cluttered or not properly distinct from the Product listing page.

## What Changes
This change will involve reorganizing the views and navigation to clearly define two sections:
1.  **Home Page (`/Home/Index`)**: Should act as a landing page greeting the user, perhaps showing featured items, website info, or banners.
2.  **Product Page (`/Product/Index`)**: Should act as the main catalog where users can browse, search, and view all available clothing items.
Navigation links (typically in the `_Layout.cshtml` header) will be updated to point users clearly to these two distinct areas.

## Capabilities

### New Capabilities
- `page-organization`: Clearer separation between the landing experience and the product browsing experience.

### Modified Capabilities
- `<existing-name>`: N/A

## Impact
- `Views/Shared/_Layout.cshtml`: Navigation menu updates.
- `Views/Home/Index.cshtml`: UI adjustments to serve as a landing page.
- `Views/Product/Index.cshtml`: Assuring it functions properly as the primary product catalog instead of an admin-only list (or creating a user-facing variation if the current one is for admins).
