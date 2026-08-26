namespace Briefcase.Maui;

public partial class AppShell : Shell
{
	private static readonly HashSet<string> PreAuthRoutes = new(StringComparer.OrdinalIgnoreCase)
	{
		"landing", "login", "signup"
	};

	public AppShell()
	{
		InitializeComponent();
		Navigated += OnShellNavigated;
	}

	// Hide the flyout on the pre-auth screens; use a locked sidebar on large
	// screens and a hamburger drawer on phones once signed in.
	private void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
	{
		var route = CurrentItem?.Route ?? string.Empty;

		if (PreAuthRoutes.Contains(route))
		{
			FlyoutBehavior = FlyoutBehavior.Disabled;
			return;
		}

		var idiom = DeviceInfo.Idiom;
		FlyoutBehavior = idiom == DeviceIdiom.Desktop || idiom == DeviceIdiom.Tablet
			? FlyoutBehavior.Locked
			: FlyoutBehavior.Flyout;
	}
}
