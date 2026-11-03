using ipr2.Domain.Entities;

namespace ipr2.Domain.Abstractions;

public interface IUnitOfWork
{
    IRepository<CarBrand> BrandRepository { get; }
    IRepository<CarAd> AdRepository { get; }

    Task SaveAllAsync();
    Task DeleteDataBaseAsync();
    Task CreateDataBaseAsync();
}
