using System.Security.Claims;

namespace User.Api.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetId(this ClaimsPrincipal user)
    {
        string? id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                 ?? user.FindFirst("sub")?.Value;
        if (Guid.TryParse(id, out var guid)) return guid;
        return null;
    }
}
