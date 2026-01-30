using MediatR;
using TierListes.Application.Common.Interfaces.Services;
using TierListes.Application.Common.Models;

namespace TierListes.Application.UseCases.TierLists.ExportTierList;

public class ExportTierListCommandHandler : IRequestHandler<ExportTierListCommand, Result<string>>
{
    private readonly IPdfGeneratorService _pdfGenerator;
    private readonly IStorageService _storageService;

    public ExportTierListCommandHandler(
        IPdfGeneratorService pdfGenerator,
        IStorageService storageService)
    {
        _pdfGenerator = pdfGenerator;
        _storageService = storageService;
    }

    public async Task<Result<string>> Handle(
        ExportTierListCommand request,
        CancellationToken cancellationToken)
    {
        var pdfData = _pdfGenerator.GeneratePdfFromImage(request.ImageData);

        var fileName = $"tierlist_{request.UserId}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";

        using var pdfStream = new MemoryStream(pdfData);
        var url = await _storageService.UploadFileAsync(
            pdfStream,
            fileName,
            "application/pdf",
            cancellationToken
        );

        return Result<string>.Success(url, 200);
    }
}
