using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using TierListes.Application.Common.Interfaces.Services;
using TierListes.Infrastructure.Configuration;

namespace TierListes.Infrastructure.Services;

public class MinioStorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly MinioSettings _settings;

    public MinioStorageService(IAmazonS3 s3Client, IOptions<MinioSettings> settings)
    {
        _s3Client = s3Client;
        _settings = settings.Value;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync(cancellationToken);

        var putRequest = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = fileName,
            InputStream = fileStream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(putRequest, cancellationToken);

        return await GetFileUrlAsync(fileName, cancellationToken);
    }

    public Task<string> GetFileUrlAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var protocol = _settings.UseSSL ? "https" : "http";
        var url = $"{protocol}://{_settings.Endpoint}/{_settings.BucketName}/{fileName}";
        return Task.FromResult(url);
    }

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _s3Client.GetBucketLocationAsync(_settings.BucketName, cancellationToken);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await _s3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = _settings.BucketName
            }, cancellationToken);

            await SetBucketPublicReadPolicyAsync(cancellationToken);
        }
    }

    private async Task SetBucketPublicReadPolicyAsync(CancellationToken cancellationToken)
    {
        var policy = $$"""
        {
            "Version": "2012-10-17",
            "Statement": [
                {
                    "Effect": "Allow",
                    "Principal": "*",
                    "Action": ["s3:GetObject"],
                    "Resource": ["arn:aws:s3:::{{_settings.BucketName}}/*"]
                }
            ]
        }
        """;

        await _s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest
        {
            BucketName = _settings.BucketName,
            Policy = policy
        }, cancellationToken);
    }
}
