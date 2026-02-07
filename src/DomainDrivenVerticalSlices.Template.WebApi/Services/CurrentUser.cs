namespace DomainDrivenVerticalSlices.Template.WebApi.Services;

using System.Security.Claims;
using DomainDrivenVerticalSlices.Template.Application.Interfaces;

/// <summary>
/// Implementation of IUser that retrieves the current user from HttpContext.
/// Returns the user's ID from the claims principal, or null if not authenticated.
/// </summary>
public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    /// <inheritdoc/>
    public string? Id => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}
