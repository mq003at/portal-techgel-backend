namespace portal.Authentication.Services;

public class OwnCloudUserService
{
    private readonly OwnCloudClient _client;

    public OwnCloudUserService(OwnCloudClient client)
    {
        _client = client;
    }

    public async Task<bool> CreateUserAsync(string userId, string password)
    {
        var result = await _client.PostAsync(
            "cloud/users",
            new Dictionary<string, string> { { "userid", userId }, { "password", password } }
        );

        var xml = await _client.ReadResponseAsStringAsync(result);
        return result.IsSuccessStatusCode && xml.Contains("<status>ok</status>");
    }
}
