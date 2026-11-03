namespace ipr2;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AdDetailsPage), typeof(AdDetailsPage));
		Routing.RegisterRoute(nameof(AdEditPage), typeof(AdEditPage));
		Routing.RegisterRoute(nameof(BrandEditPage), typeof(BrandEditPage));
	}
}
