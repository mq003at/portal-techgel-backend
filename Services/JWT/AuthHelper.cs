using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

public static class AuthHelper
{
    public static async Task SignInEmployeeAsync(
        HttpContext httpContext,
        string id,
        string mainId,
        string role,
        IEnumerable<int> organizationEntityIds,
        TimeSpan? lifetime = null
    ) // e.g. 2 hours
    {
        var claims = new List<Claim>
        {
            // Standard identity claims
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Name, mainId),
            new Claim(ClaimTypes.Role, role),
            // ERP-specific extras
            new Claim("MainId", mainId ?? string.Empty),
            new Claim("Id", id ?? string.Empty),
            new Claim("Role", role ?? string.Empty),
            new Claim(
                "OrganizationEntityIds",
                string.Join(",", organizationEntityIds ?? Enumerable.Empty<int>())
            )
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(
            scheme: "Cookies",
            principal: principal,
            properties: new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow + (lifetime ?? TimeSpan.FromHours(2)),
                AllowRefresh = true
            }
        );
    }
}
