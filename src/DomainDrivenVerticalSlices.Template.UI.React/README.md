# Domain Driven Vertical Slices React UI

This project is part of the Domain Driven Vertical Slices Template, designed to provide a React UI integrated with an ASP.NET Core backend. It follows the principles of Domain-Driven Design (DDD) and Vertical Slice Architecture to ensure scalability, maintainability, and clean separation of concerns.

## Getting Started

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

### Prerequisites

Before you begin, ensure you have met the following requirements:

- You have installed Node.js and npm. You can download them from [nodejs.org](https://nodejs.org/).

### Available Scripts

In the project directory, you can run:

#### `npm start`

Runs the app in development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in your browser.

The page will reload if you make edits.\
You will also see any lint errors in the console.

#### `npm test`

Launches the test runner in interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

#### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

#### `npm run eject`

**Note: this is a one-way operation. Once you `eject`, you can't go back!**

If you aren't satisfied with the build tool and configuration choices, you can `eject` at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (webpack, Babel, ESLint, etc) directly into your project so you have full control over them. All of the commands except `eject` will still work, but they will point to the copied scripts so you can tweak them. At this point, you're on your own.

You don't have to ever use `eject`. The curated feature set is suitable for small and middle deployments, and you shouldn't feel obligated to use this feature. However, we understand that this tool wouldn't be useful if you couldn't customize it when you are ready for it.

## Current UI Functionality

This React UI is designed to interact with the backend ASP.NET Core API and provides the following features for managing `Entity1`:

- **List of Entities**: Displays a list of all `Entity1` instances fetched from the backend API.
- **Add Entity**: Provides a form to add a new instance of `Entity1`.
- **Edit Entity**: Allows users to edit an existing instance of `Entity1`.
- **Delete Entity**: Enables users to delete an instance of `Entity1` from the list.

### Components Overview

- **CirclePlusIcon**: A reusable icon component used across the application.
- **Entity1**:
  - **AddEntity1**: Component for adding a new `Entity1`.
  - **EditEntity1**: Component for editing an existing `Entity1`.
  - **Entity1Form**: Form component used by both Add and Edit components.
  - **Entity1List**: Displays a list of `Entity1` instances.
  - **Entity1ListItem**: Represents a single item in the `Entity1` list.
- **Footer**: A footer component for the application.

### Services

- **Entity1Service.js**: Contains functions to interact with the backend API for `Entity1`.

## Environment Variables

This project uses environment variables defined in `.env` files for configuration. Ensure you set up the following variables:

- **REACT_APP_API_URL**: The base URL of the backend API.

## Deployment

To deploy the application, follow these steps:

1. Build the app using `npm run build`.
2. Serve the static files from the `build` directory using a web server of your choice.

For detailed deployment instructions, see the [Create React App deployment documentation](https://facebook.github.io/create-react-app/docs/deployment).

## Learn More

You can learn more in the [Create React App documentation](https://facebook.github.io/create-react-app/docs/getting-started).

To learn React, check out the [React documentation](https://reactjs.org/).

## License

Distributed under the MIT License. See `LICENSE` for more information.
