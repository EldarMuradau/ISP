namespace ipr2.Application.AdUseCases.Commands;

public sealed record UpdateAdCommand(
    int Id,
    string Name,
    decimal Price,
    int Year,
    int Mileage,
    string Description,
    int BrandId) : IRequest<CarAd?>;

internal class UpdateAdCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateAdCommand, CarAd?>
{
    public async Task<CarAd?> Handle(UpdateAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await unitOfWork.AdRepository.GetByIdAsync(request.Id, cancellationToken);
        if (ad is null)
        {
            return null;
        }

        ad.Update(request.Name, request.Price, request.Year, request.Mileage, request.Description);
        ad.MoveToBrand(request.BrandId);
        await unitOfWork.AdRepository.UpdateAsync(ad, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return ad;
    }
}
