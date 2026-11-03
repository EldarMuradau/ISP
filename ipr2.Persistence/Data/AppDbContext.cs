namespace ipr2.Persistence.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<CarBrand> Brands => Set<CarBrand>();
    public DbSet<CarAd> Ads => Set<CarAd>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarBrand>()
            .HasMany(b => b.Ads)
            .WithOne(a => a.Brand)
            .HasForeignKey(a => a.BrandId);
    }
}
