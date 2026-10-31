using System.Windows.Input;
using MauiItemsApp.Services;
using MauiItemsApp.Views;

namespace MauiItemsApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly AuthService _authService;

    public LoginViewModel(AuthService authService)
    {
        _authService = authService;
        LoginCommand = new Command(async () => await LoginAsync());
        CancelCommand = new Command(Cancel);
    }

    private string _userName = string.Empty;
    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand CancelCommand { get; }

    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
        {
            ShowError("Login failed: please enter a user name and password.");
            return;
        }

        try
        {
            IsBusy = true;
            Message = string.Empty;

            bool authenticated = await _authService.LoginAsync(UserName.Trim(), Password);
            if (!authenticated)
            {
                ShowError($"Login failed for user \"{UserName.Trim()}\". Check your user name and password.");
                return;
            }

            Password = string.Empty;
            await Shell.Current.GoToAsync(nameof(ItemsPage));
        }
        catch (HttpRequestException)
        {
            ShowError("Login failed: the authentication service could not be reached.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void Cancel()
    {
        UserName = string.Empty;
        Password = string.Empty;
        Message = string.Empty;
    }
}
