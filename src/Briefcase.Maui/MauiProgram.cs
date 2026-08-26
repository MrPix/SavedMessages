using CommunityToolkit.Mvvm.ComponentModel;
using Briefcase.Maui.Services;
using Briefcase.Maui.ViewModels;
using Briefcase.Maui.Views;
using Microsoft.Extensions.Logging;

namespace Briefcase.Maui;

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

		// Mock data (no networking).
		builder.Services.AddSingleton<MockDataService>();

		// Pages + view models.
		builder.Services.AddTransientPage<LandingPage, LandingViewModel>();
		builder.Services.AddTransientPage<LoginPage, LoginViewModel>();
		builder.Services.AddTransientPage<SignupPage, SignupViewModel>();
		builder.Services.AddTransientPage<ClipboardPage, ClipboardViewModel>();
		builder.Services.AddTransientPage<DevicesPage, DevicesViewModel>();
		builder.Services.AddTransientPage<TransferPage, TransferViewModel>();
		builder.Services.AddTransientPage<TrashPage, TrashViewModel>();
		builder.Services.AddTransientPage<SettingsPage, SettingsViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	private static IServiceCollection AddTransientPage<TPage, TViewModel>(this IServiceCollection services)
		where TPage : ContentPage
		where TViewModel : ObservableObject
	{
		services.AddTransient(typeof(TPage));
		services.AddTransient(typeof(TViewModel));
		return services;
	}
}
