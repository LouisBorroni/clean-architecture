using MediatR;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.TierList;

namespace TierListes.Application.UseCases.TierLists.GetTierList;

public class GetTierListQueryHandler : IRequestHandler<GetTierListQuery, Result<IEnumerable<TierListDto>>>
{
    private readonly ITierListRepository _tierListRepository;

    public GetTierListQueryHandler(ITierListRepository tierListRepository)
    {
        _tierListRepository = tierListRepository;
    }

    public async Task<Result<IEnumerable<TierListDto>>> Handle(
        GetTierListQuery request,
        CancellationToken cancellationToken)
    {
        var tierLists = await _tierListRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        var dtos = tierLists.Select(tl => new TierListDto(
            tl.CompanyId,
            tl.TierLevel
        ));

        return Result<IEnumerable<TierListDto>>.Success(dtos, 200);
    }
}
