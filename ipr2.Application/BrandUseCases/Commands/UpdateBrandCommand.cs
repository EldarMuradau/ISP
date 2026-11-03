namespace ipr2.Application.BrandUseCases.Commands;

public sealed record UpdateBrandCommand(int Id, string Name, string Country, int FoundedYear) : IRequest<CarBrand?>;

internal class UpdateBrandCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateBrandCommand, CarBrand?>
{
    public async Task<CarBrand?> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await unitOfWork.BrandRepository.GetByIdAsync(request.Id, cancellationToken);
        if (brand is null)
        {
            return null;
        }

        brand.Update(request.Name, request.Country, request.FoundedYear);
        await unitOfWork.BrandRepository.UpdateAsync(brand, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return brand;
    }
}
