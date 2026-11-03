namespace ipr2.Application.BrandUseCases.Commands;

public sealed record AddBrandCommand(string Name, string Country, int FoundedYear) : IRequest<CarBrand>;

internal class AddBrandCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddBrandCommand, CarBrand>
{
    public async Task<CarBrand> Handle(AddBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = new CarBrand(request.Name, request.Country, request.FoundedYear);
        await unitOfWork.BrandRepository.AddAsync(brand, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return brand;
    }
}
