using PacManApp.Views;

namespace PacManApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
        Routing.RegisterRoute(nameof(GamePage), typeof(GamePage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}

