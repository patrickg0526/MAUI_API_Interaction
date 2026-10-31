using MauiItemsApp.Services;
using MauiItemsApp.ViewModels;
using MauiItemsApp.Views;
using Microsoft.Extensions.Logging;

namespace MauiItemsApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Data access components (web service clients)
		builder.Services.AddSingleton<AuthService>();
		builder.Services.AddSingleton<ItemService>();

		// View Models and Views, wired together through dependency injection
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<ItemsViewModel>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<ItemsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
