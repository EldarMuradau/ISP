namespace ipr2.Domain.Entities;

public class CarBrand : Entity
{
    private readonly List<CarAd> _ads = new();

    private CarBrand()
    {
    }

    public CarBrand(string name, string country, int foundedYear)
    {
        Name = name;
        Country = country;
        FoundedYear = foundedYear;
    }

    public string Name { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public int FoundedYear { get; private set; }

    public IReadOnlyList<CarAd> Ads => _ads.AsReadOnly();

    public void Update(string name, string country, int foundedYear)
    {
        Name = name;
        Country = country;
        FoundedYear = foundedYear;
    }
}
