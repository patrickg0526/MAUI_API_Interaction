namespace MauiItemsApp.Models;

// Matches the JSON returned by ItemsWebAPI's /api/Items endpoint
public class Item
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string ItemDescription { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
}
