namespace ipr2.Application.AdUseCases.Commands;

public sealed record DeleteAdCommand(int Id) : IRequest;

internal class DeleteAdCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteAdCommand>
{
    public async Task Handle(DeleteAdCommand request, CancellationToken cancellationToken)
    {
        var ad = await unitOfWork.AdRepository.GetByIdAsync(request.Id, cancellationToken);
        if (ad is null)
        {
            return;
        }

        await unitOfWork.AdRepository.DeleteAsync(ad, cancellationToken);
        await unitOfWork.SaveAllAsync();
    }
}
