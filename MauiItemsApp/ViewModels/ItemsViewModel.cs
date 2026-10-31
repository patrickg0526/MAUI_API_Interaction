using System.Collections.ObjectModel;
using System.Windows.Input;
using MauiItemsApp.Models;
using MauiItemsApp.Services;

namespace MauiItemsApp.ViewModels;

public class ItemsViewModel : BaseViewModel
{
    private readonly ItemService _itemService;

    public ItemsViewModel(ItemService itemService)
    {
        _itemService = itemService;
        SaveCommand = new Command(async () => await SaveAsync());
        LoadCommand = new Command(async () => await LoadItemsAsync());
    }

    public ObservableCollection<Item> Items { get; } = new();

    private string _itemId = string.Empty;
    public string ItemId { get => _itemId; set => SetProperty(ref _itemId, value); }

    private string _itemName = string.Empty;
    public string ItemName { get => _itemName; set => SetProperty(ref _itemName, value); }

    private string _itemDescription = string.Empty;
    public string ItemDescription { get => _itemDescription; set => SetProperty(ref _itemDescription, value); }

    public ICommand SaveCommand { get; }
    public ICommand LoadCommand { get; }

    public async Task LoadItemsAsync()
    {
        try
        {
            IsBusy = true;
            var items = await _itemService.GetItemsAsync();
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);
        }
        catch (HttpRequestException)
        {
            ShowError("Could not load items: the web service could not be reached.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveAsync()
    {
        // Every field is required; warn about the missing ones and do not save
        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(ItemId)) missing.Add("Item ID");
        if (string.IsNullOrWhiteSpace(ItemName)) missing.Add("Item Name");
        if (string.IsNullOrWhiteSpace(ItemDescription)) missing.Add("Item Description");

        if (missing.Count > 0)
        {
            ShowError($"Please enter: {string.Join(", ", missing)}. The item was not saved.");
            return;
        }

        try
        {
            IsBusy = true;
            var result = await _itemService.AddItemAsync(new Item
            {
                ItemId = ItemId.Trim(),
                ItemName = ItemName.Trim(),
                ItemDescription = ItemDescription.Trim()
            });

            if (!result.Success)
            {
                ShowError(result.Message);
                return;
            }

            ShowSuccess(result.Message);
            ItemId = ItemName = ItemDescription = string.Empty;
        }
        catch (HttpRequestException)
        {
            ShowError("Could not save: the web service could not be reached.");
            return;
        }
        finally
        {
            IsBusy = false;
        }

        // Refresh the list from the web service so it shows everything that is stored
        await LoadItemsAsync();
    }
}
