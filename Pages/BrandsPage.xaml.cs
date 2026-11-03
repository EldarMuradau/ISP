namespace ipr2.Pages;

public partial class BrandsPage : ContentPage
{
	public BrandsPage(BrandsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
