using MediatR;
using TierListes.Application.Common.Interfaces.Persistence;
using TierListes.Application.Common.Interfaces.Services;
using TierListes.Application.Common.Models;

namespace TierListes.Application.UseCases.TierLists.ExportTierList;

public class ExportTierListCommandHandler : IRequestHandler<ExportTierListCommand, Result<string>>
{
    private readonly IPdfGeneratorService _pdfGenerator;
    private readonly IStorageService _storageService;
    private readonly IUserRepository _userRepository;

    public ExportTierListCommandHandler(
        IPdfGeneratorService pdfGenerator,
        IStorageService storageService,
        IUserRepository userRepository)
    {
        _pdfGenerator = pdfGenerator;
        _storageService = storageService;
        _userRepository = userRepository;
    }

    public async Task<Result<string>> Handle(
        ExportTierListCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<string>.Failure("Utilisateur non trouvé.", 404);
        }

        if (!user.HasPaid)
        {
            return Result<string>.Failure("Paiement requis pour exporter la tier list.", 402);
        }

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
