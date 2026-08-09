namespace Files.Application.Contracts;

public interface IBlobService
{
    Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default);
    
    Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default);
    
    Task DeleteAsync(string blobName, CancellationToken ct = default);
}