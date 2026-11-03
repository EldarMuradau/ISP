namespace ipr2.Persistence.Repository;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Lazy<IRepository<CarBrand>> _brandRepository;
    private readonly Lazy<IRepository<CarAd>> _adRepository;

    public EfUnitOfWork(AppDbContext context)
    {
        _context = context;
        _brandRepository = new Lazy<IRepository<CarBrand>>(() => new EfRepository<CarBrand>(context));
        _adRepository = new Lazy<IRepository<CarAd>>(() => new EfRepository<CarAd>(context));
    }

    public IRepository<CarBrand> BrandRepository => _brandRepository.Value;
    public IRepository<CarAd> AdRepository => _adRepository.Value;

    public async Task SaveAllAsync() => await _context.SaveChangesAsync();
    public async Task DeleteDataBaseAsync() => await _context.Database.EnsureDeletedAsync();
    public async Task CreateDataBaseAsync() => await _context.Database.EnsureCreatedAsync();
}
