namespace TierListes.Application.Common.Interfaces.Services;

public interface IPdfGeneratorService
{
    byte[] GeneratePdfFromImage(byte[] imageData);
}
