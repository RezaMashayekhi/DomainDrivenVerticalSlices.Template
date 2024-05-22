# DomainDrivenVerticalSlices.Template

[![Build](https://github.com/RezaMashayekhi/DomainDrivenVerticalSlices.Template/actions/workflows/build.yml/badge.svg)](https://github.com/RezaMashayekhi/DomainDrivenVerticalSlices.Template/actions/workflows/build.yml)
[![CodeQL](https://github.com/RezaMashayekhi/DomainDrivenVerticalSlices.Template/actions/workflows/codeql.yml/badge.svg)](https://github.com/RezaMashayekhi/DomainDrivenVerticalSlices.Template/actions/workflows/codeql.yml)
[![Nuget](https://img.shields.io/nuget/v/RM.DomainDrivenVerticalSlices.Template?label=NuGet)](https://www.nuget.org/packages/RM.DomainDrivenVerticalSlices.Template)
[![Nuget](https://img.shields.io/nuget/dt/RM.DomainDrivenVerticalSlices.Template?label=Downloads)](https://www.nuget.org/packages/RM.DomainDrivenVerticalSlices.Template)

This template is designed to jumpstart the development of web APIs following the principles of Domain-Driven Design (DDD) within a vertical slice architecture. Ideal for enterprise applications where separation of concerns and scalability are key, this template simplifies the adoption of DDD and clean architecture principles, ensuring your application remains agile, testable, and easy to understand.

## Support

If you find this template helpful, consider supporting it by giving it a star. Thanks! ⭐

## Getting Started

To use this template, ensure you have the following prerequisites installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (latest version)

Install the template from NuGet:

```bash
dotnet new install RM.DomainDrivenVerticalSlices.Template::8.0.2
```

### Create a new project based on the template:

#### WebAPI Only
```bash
dotnet new ddvs -n YourProjectName --UiType None
```

#### WebAPI with React UI
```bash
dotnet new ddvs -n YourProjectName --UiType React
```

## Usage

To get started, navigate to the root directory of your generated project and start the WebApi project:

```bash
cd src/YourProjectName.WebApi
dotnet run
```

For projects including React UI:

```bash
cd src/YourProjectName.UI.React
npm install
npm start
```

**Note:** The `UiType` environment variable is used for development purposes to enable CORS configuration for a React frontend. Users creating new projects don't need to set this variable.

## Template Structure

The template is organized into several projects, each with a specific role:

*   **Application**: Contains the application logic, including DTOs, commands, queries, and event handlers.
*   **Domain**: Houses the domain entities, value objects, and domain events.
*   **Infrastructure**: Implements the application's infrastructure concerns, such as database access, file storage, etc.
*   **WebApi**: The entry point to the application, responsible for hosting the web API.
*   **Common**: Shared resources across the application, including error handling and result modeling.
*   **UI.React**: Contains the React frontend code (if included).
*   **Tests**: A suite of unit and integration tests to ensure your codebase remains reliable and maintainable.

For detailed information on each project, refer to the README.md files within their respective directories.

## Features

*   **DDD Foundations**: Leverage DDD principles to design and implement a robust, scalable application.
*   **Vertical Slices Architecture**: Organize your application into vertical slices for improved maintainability and scalability.
*   **Pre-configured CI/CD**: GitHub Actions workflows are included for building and testing your application.
*   **Comprehensive Testing**: Unit and integration tests using xUnit, Moq, and FluentAssertions.

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Development Notes

This template supports optional integration with a React frontend. For developers who are modifying the template or testing features specific to React, it is crucial to set the `UiType` environment variable accordingly:


-   **Setting the Environment Variable:**

    -   **For Windows:**

        ```bash
        set UiType=React  # Use 'None' for WebAPI only setups
        ```

        -   **Setting Permanent Environment Variables**:
            ```bash
            setx UiType React  # Sets 'UiType' permanently for the current user
            setx UiType React /M  # Sets 'UiType' permanently system-wide
            ```
            Note: `setx` changes will only affect new command prompt sessions, not the current session.

    -   **For Linux/macOS:**

        ```bash
        export UiType=React  # Use 'None' for WebAPI only setups
        ```

This setting enables CORS configurations necessary for communication between the React frontend and the backend during development.

**Important Note:** The `UiType` environment variable is specifically for development and testing within the template's context. Users creating new projects from this template do not need to set this variable unless they are also testing or developing enhancements to the template itself.

## Contributing

We welcome contributions from the community and are pleased to have you join us. If you would like to contribute to the project, refer to the following guidelines:

- **Fork and Clone**: Fork the project on GitHub, clone your fork locally and configure the upstream repo.

- **Branching**: Preferably, create a new branch for each feature or fix.

- **Code Style**: Follow the coding style used throughout the project, including indentation and comments.

- **Submitting Changes**: Submit a pull request detailing the changes proposed.

- **Reporting Bugs**: Use the GitHub Issues track to report bugs. Please ensure your description is clear and has sufficient instructions to be able to reproduce the issue.

- **Feature Requests**: Use GitHub Issues to submit feature requests, clearly explaining the rationale and potential use case.

For more details, see the guidelines for contributing in the project documentation.

