using System.Globalization;
using ipr1.Models;
using ipr1.Services;

namespace ipr1.Pages;

public partial class CurrencyConverterPage : ContentPage
{
    private readonly IRateService _rateService;

    public CurrencyConverterPage(IRateService rateService)
    {
        InitializeComponent();
        _rateService = rateService;
        RateDatePicker.Date = DateTime.Today;
        RateDatePicker.MaximumDate = DateTime.Today;
        _ = LoadRatesAsync(DateTime.Today);
    }

    private async void OnDateSelected(object? sender, DateChangedEventArgs e)
    {
        await LoadRatesAsync(e.NewDate);
    }

    private async Task LoadRatesAsync(DateTime date)
    {
        ErrorLabel.IsVisible = false;
        try
        {
            var rates = await Task.Run(() => _rateService.GetRates(date).ToList());

            var selected = (CurrencyPicker.SelectedItem as Rate)?.Abbreviation;
            RatesList.BindingContext = rates;
            CurrencyPicker.ItemsSource = rates;
            CurrencyPicker.SelectedItem = rates.FirstOrDefault(r => r.Abbreviation == selected) ?? rates.FirstOrDefault();
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = $"Не удалось загрузить курсы: {ex.Message}";
            ErrorLabel.IsVisible = true;
        }
    }

    private void OnCurrencyChanged(object? sender, EventArgs e)
    {
        if (CurrencyPicker.SelectedItem is Rate rate)
            ForeignLabel.Text = $"Сумма в {rate.Abbreviation}";
    }

    private void OnBynToForeignClicked(object? sender, EventArgs e)
    {
        if (TryGetInput(BynEntry, out decimal amount, out decimal ratePerUnit))
            ForeignEntry.Text = (amount / ratePerUnit).ToString("F4");
    }

    private void OnForeignToBynClicked(object? sender, EventArgs e)
    {
        if (TryGetInput(ForeignEntry, out decimal amount, out decimal ratePerUnit))
            BynEntry.Text = (amount * ratePerUnit).ToString("F4");
    }

    // ratePerUnit = BYN for one unit of the selected currency
    private bool TryGetInput(Entry entry, out decimal amount, out decimal ratePerUnit)
    {
        amount = 0;
        ratePerUnit = 0;

        if (CurrencyPicker.SelectedItem is not Rate rate || rate.OfficialRate is not decimal official)
        {
            DisplayAlert("Ошибка", "Выберите валюту", "OK");
            return false;
        }

        string text = (entry.Text ?? "").Replace(',', '.');
        if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
        {
            DisplayAlert("Ошибка", "Введите корректную сумму", "OK");
            return false;
        }

        ratePerUnit = official / rate.Scale;
        return true;
    }
}
