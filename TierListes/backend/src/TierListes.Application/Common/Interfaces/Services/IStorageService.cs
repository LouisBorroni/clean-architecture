namespace TierListes.Application.Common.Interfaces.Services;

public interface IStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task<string> GetFileUrlAsync(string fileName, CancellationToken cancellationToken = default);
}
