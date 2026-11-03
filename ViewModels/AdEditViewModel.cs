using ipr2.Application.AdUseCases.Commands;
using ipr2.Application.BrandUseCases.Queries;

namespace ipr2.ViewModels;

[QueryProperty(nameof(Ad), "Ad")]
[QueryProperty(nameof(BrandId), "BrandId")]
public partial class AdEditViewModel : ObservableObject
{
	private readonly IMediator _mediator;

	public AdEditViewModel(IMediator mediator)
	{
		_mediator = mediator;
	}

	public ObservableCollection<CarBrand> Brands { get; } = new();

	[ObservableProperty]
	CarAd? ad;

	[ObservableProperty]
	int brandId;

	[ObservableProperty]
	string title = "New ad";

	[ObservableProperty]
	string name = string.Empty;

	[ObservableProperty]
	decimal price;

	[ObservableProperty]
	int year = DateTime.Now.Year;

	[ObservableProperty]
	int mileage;

	[ObservableProperty]
	string description = string.Empty;

	[ObservableProperty]
	CarBrand? selectedBrand;

	partial void OnAdChanged(CarAd? value)
	{
		if (value is null) return;
		Title = "Edit ad";
		Name = value.Name;
		Price = value.Price;
		Year = value.Year;
		Mileage = value.Mileage;
		Description = value.Description;
		BrandId = value.BrandId ?? 0;
	}

	[RelayCommand]
	async Task LoadBrands()
	{
		var brands = await _mediator.Send(new GetAllBrandsRequest());
		Brands.Clear();
		foreach (var brand in brands)
			Brands.Add(brand);
		SelectedBrand = Brands.FirstOrDefault(b => b.Id == BrandId);
	}

	[RelayCommand]
	async Task Save()
	{
		if (string.IsNullOrWhiteSpace(Name) || SelectedBrand is null)
		{
			await Shell.Current.DisplayAlert("Error", "Fill in the model name and choose a brand", "OK");
			return;
		}

		if (Ad is null)
			await _mediator.Send(new AddAdCommand(Name, Price, Year, Mileage, Description, SelectedBrand.Id));
		else
			await _mediator.Send(new UpdateAdCommand(Ad.Id, Name, Price, Year, Mileage, Description, SelectedBrand.Id));

		await Shell.Current.GoToAsync("..");
	}
}
