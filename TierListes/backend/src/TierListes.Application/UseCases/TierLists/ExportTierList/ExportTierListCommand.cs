using MediatR;
using TierListes.Application.Common.Models;

namespace TierListes.Application.UseCases.TierLists.ExportTierList;

public record ExportTierListCommand(
    Guid UserId,
    byte[] ImageData
) : IRequest<Result<string>>;
