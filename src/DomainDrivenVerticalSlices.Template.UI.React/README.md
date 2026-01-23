# Domain Driven Vertical Slices React UI

This project is part of the Domain Driven Vertical Slices Template, designed to provide a React UI integrated with an ASP.NET Core backend. It follows the principles of Domain-Driven Design (DDD) and Vertical Slice Architecture to ensure scalability, maintainability, and clean separation of concerns.

## Getting Started

This project uses [Vite](https://vitejs.dev/) as the build tool for a faster and more efficient development experience.

### Prerequisites

Before you begin, ensure you have met the following requirements:

-   Install [Node.js 20.19+ or 22.12+](https://nodejs.org/) (which brings npm 10+) to match the toolchain used by the .NET 10 template. Earlier 20.x releases can cause build warnings with the SWC-powered Vite plugin.

### Available Scripts

In the project directory, you can run:

#### `npm run dev`

Runs the app in development mode.\
Open [http://localhost:5173](http://localhost:5173) to view it in your browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

#### `npm test`

Launches the test runner in interactive watch mode.\
Tests are run using [Vitest](https://vitest.dev/), a modern test runner built on top of Vite.

#### `npm run build`

Builds the app for production to the `dist` folder using Vite's build process.\

#### `npm run preview`

Locally previews the production build from `dist/`.

#### `npm run lint`

Runs ESLint against all source files and fails if any warnings are detected. This is the same check executed in CI.

## Current UI Functionality

This React UI is designed to interact with the backend ASP.NET Core API and provides the following features for managing `Entity1`:

-   **List of Entities**: Displays a list of all `Entity1` instances fetched from the backend API with pagination.
-   **Add Entity**: Provides a form to add a new instance of `Entity1` with confirmation modals.
-   **Edit Entity**: Allows users to edit an existing instance of `Entity1`.
-   **Delete Entity**: Enables users to delete an instance of `Entity1` with confirmation modal.
-   **Dark Mode**: Toggle between light and dark themes with localStorage persistence.
-   **Search**: Filter entities by property value.

### Components Overview

-   **Layout Components** (`src/layout/`):

    -   **Layout**: Main wrapper component with navigation and footer.
    -   **NavMenu**: Navigation bar with dark mode toggle.
    -   **Footer**: Application footer.
    -   **PageNotFound**: 404 error page.

-   **Modal Components** (`src/components/modals/`):

    -   **DeleteModal**: Confirmation dialog for delete actions.
    -   **CancelModal**: Confirmation dialog for discarding changes.
    -   **SaveModal**: Success confirmation after save operations.
    -   **ErrorModal**: Error message display.

-   **Common Components** (`src/components/common/`):

    -   **Button**: Reusable button with variants (filled, outlined, text, danger).

-   **Entity1 Components** (`src/components/Entity1/`):

    -   **Entity1Form**: Unified form for adding and editing entities.
    -   **Entity1List**: Displays a paginated list of entities with search.
    -   **Entity1ListItem**: Represents a single item in the list.

-   **Context** (`src/context/`):
    -   **ThemeContext**: Global dark mode state management with localStorage persistence.

### Services

-   **Entity1Service.js**: Contains functions to interact with the backend API for `Entity1`.

### Testing

-   **MSW (Mock Service Worker)**: API mocking for tests (`src/mocks/`).
-   **Test Utilities**: Custom render functions with providers (`src/utils/test-utils.jsx`).

### Styling

This project uses **Tailwind CSS v4** for styling with:

-   Utility-first CSS classes
-   Dark mode support via class-based toggling
-   CSS variables for theming
-   **@headlessui/react** for accessible modal components
-   **@heroicons/react** for SVG icons

## Environment Variables

This project uses environment variables defined in `.env` files for configuration. Ensure you set up the following variables:

-   **VITE_API_BASE_URL**: The base path for the `Entity1` API (defaults to `/api/entity1`). The Vite dev server proxies `/api` to `http://localhost:5246` by default.

Copy `.env.development` to `.env` (or vice versa) and adjust the value when targeting different environments.

## Deployment

To deploy the application, follow these steps:

1. Build the app using `npm run build`.
2. The build will create a `dist` directory with your built application.
3. Deploy the contents of the `dist` directory to your web server.

For detailed deployment instructions and options for different hosting platforms, see the [Vite deployment guide](https://vitejs.dev/guide/static-deploy.html).

## Learn More

To learn more about the tools used in this project:

-   [Vite Documentation](https://vitejs.dev/)
-   [React Documentation](https://react.dev/)
-   [Vitest Documentation](https://vitest.dev/)

## License

Distributed under the MIT License. See `LICENSE` for more information.
