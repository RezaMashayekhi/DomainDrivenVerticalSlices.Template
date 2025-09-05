# Domain Driven Vertical Slices React UI

This project is part of the Domain Driven Vertical Slices Template, designed to provide a React UI integrated with an ASP.NET Core backend. It follows the principles of Domain-Driven Design (DDD) and Vertical Slice Architecture to ensure scalability, maintainability, and clean separation of concerns.

## Getting Started

This project uses [Vite](https://vitejs.dev/) as the build tool for a faster and more efficient development experience.

### Prerequisites

Before you begin, ensure you have met the following requirements:

-   You have installed Node.js and npm. You can download them from [nodejs.org](https://nodejs.org/).

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

## Current UI Functionality

This React UI is designed to interact with the backend ASP.NET Core API and provides the following features for managing `Entity1`:

-   **List of Entities**: Displays a list of all `Entity1` instances fetched from the backend API.
-   **Add Entity**: Provides a form to add a new instance of `Entity1`.
-   **Edit Entity**: Allows users to edit an existing instance of `Entity1`.
-   **Delete Entity**: Enables users to delete an instance of `Entity1` from the list.

### Components Overview

-   **CirclePlusIcon**: A reusable icon component used across the application.
-   **Entity1**:
    -   **AddEntity1**: Component for adding a new `Entity1`.
    -   **EditEntity1**: Component for editing an existing `Entity1`.
    -   **Entity1Form**: Form component used by both Add and Edit components.
    -   **Entity1List**: Displays a list of `Entity1` instances.
    -   **Entity1ListItem**: Represents a single item in the `Entity1` list.
-   **Footer**: A footer component for the application.

### Services

-   **Entity1Service.js**: Contains functions to interact with the backend API for `Entity1`.

## Environment Variables

This project uses environment variables defined in `.env` files for configuration. Ensure you set up the following variables:

-   **VITE_API_URL**: The base URL of the backend API.

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
