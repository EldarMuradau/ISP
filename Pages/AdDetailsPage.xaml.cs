namespace ipr2.Pages;

public partial class AdDetailsPage : ContentPage
{
	public AdDetailsPage(AdDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
