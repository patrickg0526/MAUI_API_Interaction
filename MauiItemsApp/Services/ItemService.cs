using System.Text;
using MauiItemsApp.Models;
using Newtonsoft.Json;

namespace MauiItemsApp.Services;

public record SaveResult(bool Success, string Message);

// Data access component for item storage and retrieval through the web service
public class ItemService
{
    private static HttpClient CreateClient()
    {
        var client = new HttpClient { BaseAddress = new Uri(ApiSettings.BaseAddress) };
        client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
        return client;
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        using var client = CreateClient();
        var response = await client.GetAsync("api/Items");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Item>>(json) ?? new List<Item>();
    }

    public async Task<SaveResult> AddItemAsync(Item item)
    {
        using var client = CreateClient();
        var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/Items", content);

        if (response.IsSuccessStatusCode)
            return new SaveResult(true, $"Item \"{item.ItemName}\" was saved.");

        // The API explains validation problems (400) and duplicate IDs (409) in the response body
        string error = ReadErrorMessage(await response.Content.ReadAsStringAsync());
        return new SaveResult(false, string.IsNullOrWhiteSpace(error) ? $"Save failed ({(int)response.StatusCode})." : error);
    }

    // Error bodies may arrive as a JSON-encoded string ("...") or as plain text
    private static string ReadErrorMessage(string body)
    {
        if (body.StartsWith('"'))
        {
            try { return JsonConvert.DeserializeObject<string>(body) ?? string.Empty; }
            catch (JsonException) { }
        }
        return body;
    }
}
