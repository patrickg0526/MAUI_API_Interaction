using System.Net.Http.Headers;
using System.Text;

namespace MauiItemsApp.Services;

// Data access component for authentication: sends the credentials to the web service using Basic Authentication
public class AuthService
{
    public async Task<bool> LoginAsync(string userName, string password)
    {
        using var client = new HttpClient { BaseAddress = new Uri(ApiSettings.BaseAddress) };

        string token = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);

        var response = await client.GetAsync("api/Auth");
        return response.IsSuccessStatusCode;
    }
}
