namespace ipr2.Pages;

public partial class BrandEditPage : ContentPage
{
	public BrandEditPage(BrandEditViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
