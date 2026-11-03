namespace ipr2;

// Fully qualified because the ipr2.Application namespace shadows Microsoft.Maui.Controls.Application
public partial class App : Microsoft.Maui.Controls.Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
