using MediatR;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Application.Common.Models;
using TierListes.Domain.Entities;

namespace TierListes.Application.UseCases.TierLists.SaveTierList;

public class SaveTierListCommandHandler : IRequestHandler<SaveTierListCommand, Result<bool>>
{
    private readonly ITierListRepository _tierListRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SaveTierListCommandHandler(
        ITierListRepository tierListRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _tierListRepository = tierListRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(
        SaveTierListCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<bool>.Unauthorized("Utilisateur non trouvé.");
        }

        if (!user.HasPaid)
        {
            return Result<bool>.Failure("Paiement requis pour sauvegarder la tier list.", 402);
        }

        await _tierListRepository.DeleteByUserIdAsync(request.UserId, cancellationToken);

        foreach (var ranking in request.Rankings)
        {
            var tierList = TierList.Create(
                request.UserId,
                ranking.CompanyId,
                ranking.TierLevel,
                ranking.Position
            );
            await _tierListRepository.AddAsync(tierList, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, 200);
    }
}
