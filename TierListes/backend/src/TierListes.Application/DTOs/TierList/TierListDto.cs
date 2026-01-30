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

public record ExportTierListRequestDto(
    string ImageBase64
);

public record ExportResponseDto(
    string PdfUrl
);
