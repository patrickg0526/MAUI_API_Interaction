using MauiItemsApp.Views;

namespace MauiItemsApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// The data entry page is pushed on top of the login page after a successful login
		Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));
	}
}
