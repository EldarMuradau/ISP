using ipr2.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ipr2.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, DbContextOptions options)
    {
        services
            .AddSingleton(new AppDbContext((DbContextOptions<AppDbContext>)options))
            .AddSingleton<IUnitOfWork, EfUnitOfWork>();
        return services;
    }
}
