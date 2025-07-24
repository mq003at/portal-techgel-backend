using System.Net.Http.Headers;
using System.Text;
using portal.Models;

namespace portal.Authentication.Services;

public class OwnCloudClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public OwnCloudClient(OwnCloudSettings settings)
    {
        _baseUrl = settings.BaseUrl.TrimEnd('/');
        _httpClient = new HttpClient();

        var byteArray = Encoding.ASCII.GetBytes(
            $"{settings.AdminUsername}:{settings.AdminPassword}"
        );
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(byteArray)
        );

        _httpClient.DefaultRequestHeaders.Add("OCS-APIRequest", "true");
    }

    public async Task<HttpResponseMessage> PostAsync(
        string relativeUrl,
        Dictionary<string, string> formData
    )
    {
        var content = new FormUrlEncodedContent(formData);
        return await _httpClient.PostAsync($"{_baseUrl}/{relativeUrl}", content);
    }

    public async Task<HttpResponseMessage> GetAsync(string relativeUrl) =>
        await _httpClient.GetAsync($"{_baseUrl}/{relativeUrl}");

    public async Task<HttpResponseMessage> DeleteAsync(string relativeUrl) =>
        await _httpClient.DeleteAsync($"{_baseUrl}/{relativeUrl}");

    public async Task<string> ReadResponseAsStringAsync(HttpResponseMessage response) =>
        await response.Content.ReadAsStringAsync();
}
