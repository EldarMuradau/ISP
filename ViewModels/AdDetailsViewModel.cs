using ipr2.Application.AdUseCases.Commands;

namespace ipr2.ViewModels;

[QueryProperty(nameof(Ad), "Ad")]
public partial class AdDetailsViewModel : ObservableObject
{
	private readonly IMediator _mediator;

	public AdDetailsViewModel(IMediator mediator)
	{
		_mediator = mediator;
	}

	[ObservableProperty]
	CarAd? ad;

	[RelayCommand]
	void Refresh() => OnPropertyChanged(nameof(Ad));

	[RelayCommand]
	async Task Edit()
	{
		if (Ad is null) return;
		await Shell.Current.GoToAsync(nameof(AdEditPage), new Dictionary<string, object> { ["Ad"] = Ad });
	}

	[RelayCommand]
	async Task Delete()
	{
		if (Ad is null) return;
		if (!await Shell.Current.DisplayAlert("Delete", $"Delete ad \"{Ad.Name}\"?", "Yes", "No"))
			return;

		await _mediator.Send(new DeleteAdCommand(Ad.Id));
		ImageStorage.Delete(Ad.Id);
		await Shell.Current.GoToAsync("..");
	}

	[RelayCommand]
	async Task PickImage()
	{
		if (Ad is null) return;

		var file = await FilePicker.Default.PickAsync(new PickOptions
		{
			PickerTitle = "Select car photo",
			FileTypes = FilePickerFileType.Images
		});
		if (file is null) return;

		await ImageStorage.SaveAsync(Ad.Id, file);
		OnPropertyChanged(nameof(Ad));
	}
}
