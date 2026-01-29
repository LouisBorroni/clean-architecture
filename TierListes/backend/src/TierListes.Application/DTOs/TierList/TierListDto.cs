namespace TierListes.Application.DTOs.TierList;

public record TierListDto(
    Guid CompanyId,
    string TierLevel
);

public record SaveTierListRequestDto(
    List<TierListItemDto> Rankings
);

public record TierListItemDto(
    Guid CompanyId,
    string TierLevel
);
