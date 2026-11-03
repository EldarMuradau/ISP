using ipr2.Application.AdUseCases.Queries;
using ipr2.Application.BrandUseCases.Queries;

namespace ipr2.ViewModels;

public partial class BrandsViewModel : ObservableObject
{
	private readonly IMediator _mediator;

	public BrandsViewModel(IMediator mediator)
	{
		_mediator = mediator;
	}

	public ObservableCollection<CarBrand> Brands { get; } = new();

	[ObservableProperty]
	ObservableCollection<CarAd> ads = new();

	[ObservableProperty]
	CarBrand? selectedBrand;

	[RelayCommand]
	async Task UpdateGroupList() => await GetBrands();

	[RelayCommand]
	async Task UpdateMembersList() => await GetAds();

	[RelayCommand]
	async Task ShowDetails(CarAd ad) =>
		await Shell.Current.GoToAsync(nameof(AdDetailsPage), new Dictionary<string, object> { ["Ad"] = ad });

	[RelayCommand]
	async Task AddBrand() => await Shell.Current.GoToAsync(nameof(BrandEditPage));

	[RelayCommand]
	async Task EditBrand()
	{
		if (SelectedBrand is null)
		{
            return;
        }
		await Shell.Current.GoToAsync(nameof(BrandEditPage),
			new Dictionary<string, object> { ["Brand"] = SelectedBrand });
	}

	[RelayCommand]
	async Task AddAd()
	{
		if (SelectedBrand is null)
		{
			await Shell.Current.DisplayAlert("No brand", "Select a brand first", "OK");
			return;
		}
		await Shell.Current.GoToAsync(nameof(AdEditPage),
			new Dictionary<string, object> { ["BrandId"] = SelectedBrand.Id });
	}

	public async Task GetBrands()
	{
		var selectedId = SelectedBrand?.Id;
		var brands = await _mediator.Send(new GetAllBrandsRequest());
		await MainThread.InvokeOnMainThreadAsync(() =>
		{
			SelectedBrand = null;
			Brands.Clear();
			foreach (var brand in brands)
				Brands.Add(brand);
			SelectedBrand = Brands.FirstOrDefault(b => b.Id == selectedId) ?? Brands.FirstOrDefault();
		});
	}

	public async Task GetAds()
	{
		if (SelectedBrand is null)
		{
			Ads = new();
			return;
		}

		var ads = await _mediator.Send(new GetAdsByBrandRequest(SelectedBrand.Id));
		await MainThread.InvokeOnMainThreadAsync(() => Ads = new(ads));
	}
}
