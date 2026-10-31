using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MauiItemsApp.ViewModels;

// Shared change notification so the Views update automatically through data binding
public abstract class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
                OnPropertyChanged(nameof(IsNotBusy));
        }
    }

    public bool IsNotBusy => !IsBusy;

    private string _message = string.Empty;
    public string Message
    {
        get => _message;
        set
        {
            if (SetProperty(ref _message, value))
                OnPropertyChanged(nameof(HasMessage));
        }
    }

    public bool HasMessage => !string.IsNullOrEmpty(Message);

    private Color _messageColor = Colors.OrangeRed;
    public Color MessageColor
    {
        get => _messageColor;
        set => SetProperty(ref _messageColor, value);
    }

    protected void ShowError(string text) { MessageColor = Colors.OrangeRed; Message = text; }
    protected void ShowSuccess(string text) { MessageColor = Colors.LimeGreen; Message = text; }
}
