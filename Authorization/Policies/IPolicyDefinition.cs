using Microsoft.AspNetCore.Authorization;

namespace portal.Authorization;

public interface IPolicyDefinition
{
    void Define(AuthorizationOptions options);
}
