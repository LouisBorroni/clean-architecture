using MediatR;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Application.Common.Models;
using TierListes.Domain.Entities;

namespace TierListes.Application.UseCases.TierLists.SaveTierList;

public class SaveTierListCommandHandler : IRequestHandler<SaveTierListCommand, Result<bool>>
{
    private readonly ITierListRepository _tierListRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SaveTierListCommandHandler(
        ITierListRepository tierListRepository,
        IUnitOfWork unitOfWork)
    {
        _tierListRepository = tierListRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(
        SaveTierListCommand request,
        CancellationToken cancellationToken)
    {
        await _tierListRepository.DeleteByUserIdAsync(request.UserId, cancellationToken);

        foreach (var ranking in request.Rankings)
        {
            var tierList = TierList.Create(
                request.UserId,
                ranking.CompanyId,
                ranking.TierLevel
            );
            await _tierListRepository.AddAsync(tierList, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, 200);
    }
}
