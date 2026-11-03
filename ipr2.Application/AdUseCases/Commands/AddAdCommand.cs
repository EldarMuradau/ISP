namespace ipr2.Application.AdUseCases.Commands;

public sealed record AddAdCommand(
    string Name,
    decimal Price,
    int Year,
    int Mileage,
    string Description,
    int BrandId) : IRequest<CarAd>;

internal class AddAdCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddAdCommand, CarAd>
{
    public async Task<CarAd> Handle(AddAdCommand request, CancellationToken cancellationToken)
    {
        var ad = new CarAd(request.Name, request.Price, request.Year, request.Mileage, request.Description);
        ad.MoveToBrand(request.BrandId);
        await unitOfWork.AdRepository.AddAsync(ad, cancellationToken);
        await unitOfWork.SaveAllAsync();
        return ad;
    }
}
