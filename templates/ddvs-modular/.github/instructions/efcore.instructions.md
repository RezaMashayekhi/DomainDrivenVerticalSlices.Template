# EF Core Instructions

Apply these instructions when modifying persistence code.

- Put entity configurations in the module Infrastructure folder.
- Keep domain entities free of EF Core dependencies.
- Use explicit configurations instead of relying on conventions for important fields.
- Prefer real database integration tests for persistence behavior.
- Avoid generic repositories.
- Use repositories only when they represent meaningful domain-specific persistence operations.
