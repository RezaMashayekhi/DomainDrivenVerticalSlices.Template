# DomainDrivenVerticalSlices.Template

A .NET modular monolith API template organized around business modules and vertical slices.

Install the SDK specified in this project's `global.json` before restoring or building.

## Run

```bash
dotnet run --project src/DomainDrivenVerticalSlices.Template.Api
```

<!--#if (USE_ASPIRE) -->
Or run the Aspire AppHost:

```bash
dotnet run --project src/DomainDrivenVerticalSlices.Template.AppHost
```
<!--#endif -->

## Test

```bash
dotnet test DomainDrivenVerticalSlices.Template.slnx
```
