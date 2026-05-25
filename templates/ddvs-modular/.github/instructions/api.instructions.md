# API Instructions

Apply these instructions when modifying API endpoints.

- Keep endpoints thin.
- Put route mapping in endpoint classes.
- Put use-case workflow in feature handlers.
- Do not query EF Core directly from endpoint classes.
- Return typed success responses where practical.
- Map known failures to ProblemDetails.
- Do not expose EF Core entities as API responses.
