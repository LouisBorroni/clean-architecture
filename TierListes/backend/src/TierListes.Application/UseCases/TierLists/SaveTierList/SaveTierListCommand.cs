using MediatR;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.TierList;

namespace TierListes.Application.UseCases.TierLists.SaveTierList;

public record SaveTierListCommand(
    Guid UserId,
    List<TierListItemDto> Rankings
) : IRequest<Result<bool>>;
