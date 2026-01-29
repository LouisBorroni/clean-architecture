using MediatR;
using TierListes.Application.Common.Models;
using TierListes.Application.DTOs.TierList;

namespace TierListes.Application.UseCases.TierLists.GetTierList;

public record GetTierListQuery(Guid UserId) : IRequest<Result<IEnumerable<TierListDto>>>;
