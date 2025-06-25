using System.Security.Claims;

namespace portal.Services.JWT;

    public interface IJwtService
    {
        string GenerateToken(string id, string mainId, string role, List<int> organizationEntityIds);
        ClaimsPrincipal? ValidateToken(string token);
    }
