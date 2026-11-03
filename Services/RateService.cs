using System.Net.Http.Json;
using ipr1.Models;

namespace ipr1.Services;

public class RateService : IRateService
{
    private static readonly string[] Currencies = { "RUB", "EUR", "USD", "CHF", "CNY", "GBP" };

    private readonly HttpClient _httpClient;

    public RateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public IEnumerable<Rate> GetRates(DateTime date)
    {
        string url = $"?ondate={date:yyyy-MM-dd}&periodicity=0";

        var rates = _httpClient.GetFromJsonAsync<List<Rate>>(url).GetAwaiter().GetResult() ?? new List<Rate>();

        return rates.Where(r => Currencies.Contains(r.Abbreviation));
    }
}
