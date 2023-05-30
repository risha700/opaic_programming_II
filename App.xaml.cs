namespace BallBreaker;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	private void SetWindowSize()
	{
        // Get display size
        var displayInfo = DeviceDisplay.Current.MainDisplayInfo;

        // Center the window
        Shell.Current.Window.X = (displayInfo.Width / displayInfo.Density - Shell.Current.Width) / 2;
        Shell.Current.Window.Y = (displayInfo.Height / displayInfo.Density - Shell.Current.Window.Height) / 2;
    }
}
