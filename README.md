# DomainDrivenVerticalSlices.Template

[![Build](https://github.com/RezaMashayekhi/DomainDrivenVerticalSlices.Template/actions/workflows/build.yml/badge.svg)](https://github.com/RezaMashayekhi/DomainDrivenVerticalSlices.Template/actions/workflows/build.yml)
[![CodeQL](https://github.com/RezaMashayekhi/DomainDrivenVerticalSlices.Template/actions/workflows/codeql.yml/badge.svg)](https://github.com/RezaMashayekhi/DomainDrivenVerticalSlices.Template/actions/workflows/codeql.yml)
[![Nuget](https://img.shields.io/nuget/v/DomainDrivenVerticalSlices.Template?label=NuGet)](https://www.nuget.org/packages/DomainDrivenVerticalSlices.Template)
[![Nuget](https://img.shields.io/nuget/dt/DomainDrivenVerticalSlices.Template?label=Downloads)](https://www.nuget.org/packages/DomainDrivenVerticalSlices.Template)

This template is designed to jumpstart the development of web APIs following the principles of Domain-Driven Design (DDD) within a vertical slice architecture. Ideal for enterprise applications where separation of concerns and scalability are key, this template simplifies the adoption of DDD and clean architecture principles, ensuring your application remains agile, testable, and easy to understand.

## Support

If you find this template helpful, consider supporting it by giving it a star. Thanks! ⭐

## Getting Started

To use this template, ensure you have the following prerequisites installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (latest version)

Install the template from NuGet:

```bash
dotnet new install RM.DomainDrivenVerticalSlices.Template::8.0.0
```

Create a new project based on the template:

```bash
dotnet new ddvs -n YourProjectName
```

## Usage

To get started, navigate to the root directory of your generated project and start the WebApi project:

```bash
cd src/YourProjectName.WebApi
dotnet run
```

## Template Structure

The template is organized into several projects, each with a specific role:

*   **Application**: Contains the application logic, including DTOs, commands, queries, and event handlers.
*   **Domain**: Houses the domain entities, value objects, and domain events.
*   **Infrastructure**: Implements the application's infrastructure concerns, such as database access, file storage, etc.
*   **WebApi**: The entry point to the application, responsible for hosting the web API.
*   **Common**: Shared resources across the application, including error handling and result modeling.
*   **Tests**: A suite of unit and integration tests to ensure your codebase remains reliable and maintainable.

For detailed information on each project, refer to the README.md files within their respective directories.

## Features

*   **DDD Foundations**: Leverage DDD principles to design and implement a robust, scalable application.
*   **Vertical Slices Architecture**: Organize your application into vertical slices for improved maintainability and scalability.
*   **Pre-configured CI/CD**: GitHub Actions workflows are included for building and testing your application.
*   **Comprehensive Testing**: Unit and integration tests using xUnit, Moq, and FluentAssertions.

## License

Distributed under the MIT License. See `LICENSE` for more information.