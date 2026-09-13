using Files.Infrastructure.DTOs;

namespace Files.Application.Contracts;

public interface IBlobService
{
    Task<BlobUploadResult> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default, bool overwrite = true);
    
    Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default);
    
    Task DeleteAsync(string blobName, CancellationToken ct = default);
}