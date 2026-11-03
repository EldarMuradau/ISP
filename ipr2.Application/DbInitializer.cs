using Microsoft.Extensions.DependencyInjection;

namespace ipr2.Application;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider services)
    {
        var unitOfWork = services.GetRequiredService<IUnitOfWork>();

        await unitOfWork.CreateDataBaseAsync();

        if ((await unitOfWork.BrandRepository.ListAllAsync()).Count > 0)
        {
            return;
        }

        var toyota = new CarBrand("Toyota", "Japan", 1937);
        var bmw = new CarBrand("BMW", "Germany", 1916);
        var lada = new CarBrand("Lada", "Russia", 1966);
        await unitOfWork.BrandRepository.AddAsync(toyota);
        await unitOfWork.BrandRepository.AddAsync(bmw);
        await unitOfWork.BrandRepository.AddAsync(lada);
        await unitOfWork.SaveAllAsync();

        var ads = new (CarBrand Brand, CarAd Ad)[]
        {
            (toyota, new CarAd("Camry 2.5", 24500, 2019, 68000, "One owner, full service history")),
            (toyota, new CarAd("Land Cruiser 200", 52000, 2018, 120000, "Diesel, 7 seats")),
            (toyota, new CarAd("Corolla 1.6", 13900, 2016, 95000, "Economical city car")),
            (toyota, new CarAd("RAV4 Hybrid", 33500, 2021, 41000, "AWD, hybrid")),
            (bmw, new CarAd("320d", 18700, 2017, 150000, "Sport package")),
            (bmw, new CarAd("X5 xDrive40i", 61000, 2020, 54000, "Panoramic roof")),
            (bmw, new CarAd("520i", 27900, 2019, 88000, "Leather interior")),
            (lada, new CarAd("Vesta 1.6", 9800, 2020, 47000, "Manual gearbox")),
            (lada, new CarAd("Niva Legend", 8500, 2021, 23000, "4x4")),
            (lada, new CarAd("Granta", 6200, 2019, 76000, "Winter tires included")),
        };

        foreach (var (brand, ad) in ads)
        {
            ad.MoveToBrand(brand.Id);
            await unitOfWork.AdRepository.AddAsync(ad);
        }
        await unitOfWork.SaveAllAsync();
    }
}
