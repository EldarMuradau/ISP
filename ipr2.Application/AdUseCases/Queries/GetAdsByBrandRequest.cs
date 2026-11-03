namespace ipr2.Application.AdUseCases.Queries;

public sealed record GetAdsByBrandRequest(int BrandId) : IRequest<IEnumerable<CarAd>>;

internal class GetAdsByBrandRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAdsByBrandRequest, IEnumerable<CarAd>>
{
    public async Task<IEnumerable<CarAd>> Handle(GetAdsByBrandRequest request, CancellationToken cancellationToken)
    {
        return await unitOfWork.AdRepository.ListAsync(a => a.BrandId == request.BrandId, cancellationToken);
    }
}
