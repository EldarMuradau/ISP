namespace ipr2.Pages;

public partial class AdEditPage : ContentPage
{
	public AdEditPage(AdEditViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
