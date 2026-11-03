namespace ipr2.Application.BrandUseCases.Queries;

public sealed record GetAllBrandsRequest : IRequest<IEnumerable<CarBrand>>;

internal class GetAllBrandsRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetAllBrandsRequest, IEnumerable<CarBrand>>
{
    public async Task<IEnumerable<CarBrand>> Handle(GetAllBrandsRequest request, CancellationToken cancellationToken)
    {
        return await unitOfWork.BrandRepository.ListAllAsync(cancellationToken);
    }
}
