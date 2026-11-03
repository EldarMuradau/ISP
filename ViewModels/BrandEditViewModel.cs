using ipr2.Application.BrandUseCases.Commands;

namespace ipr2.ViewModels;

[QueryProperty(nameof(Brand), "Brand")]
public partial class BrandEditViewModel : ObservableObject
{
	private readonly IMediator _mediator;

	public BrandEditViewModel(IMediator mediator)
	{
		_mediator = mediator;
	}

	[ObservableProperty]
	CarBrand? brand;

	[ObservableProperty]
	string title = "New brand";

	[ObservableProperty]
	string name = string.Empty;

	[ObservableProperty]
	string country = string.Empty;

	[ObservableProperty]
	int foundedYear = 1900;

	partial void OnBrandChanged(CarBrand? value)
	{
		if (value is null) return;
		Title = "Edit brand";
		Name = value.Name;
		Country = value.Country;
		FoundedYear = value.FoundedYear;
	}

	[RelayCommand]
	async Task Save()
	{
		if (string.IsNullOrWhiteSpace(Name))
		{
			await Shell.Current.DisplayAlert("Error", "Fill in the brand name", "OK");
			return;
		}

		if (Brand is null)
		{
			await _mediator.Send(new AddBrandCommand(Name, Country, FoundedYear));
		}
		else
		{
			await _mediator.Send(new UpdateBrandCommand(Brand.Id, Name, Country, FoundedYear));
		}
		await Shell.Current.GoToAsync("..");
	}
}
