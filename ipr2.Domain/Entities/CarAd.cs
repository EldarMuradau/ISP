namespace ipr2.Domain.Entities;

public class CarAd : Entity
{
    private CarAd()
    {
    }

    public CarAd(string name, decimal price, int year, int mileage, string description)
    {
        Update(name, price, year, mileage, description);
    }

    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Year { get; private set; }
    public int Mileage { get; private set; }
    public string Description { get; private set; } = string.Empty;

    public int? BrandId { get; private set; }
    public CarBrand? Brand { get; private set; }

    public void Update(string name, decimal price, int year, int mileage, string description)
    {
        Name = name;
        Price = price < 0 ? 0 : price;
        Year = year;
        Mileage = mileage < 0 ? 0 : mileage;
        Description = description;
    }

    public void MoveToBrand(int brandId)
    {
        if (brandId <= 0) return;
        BrandId = brandId;
    }
}
