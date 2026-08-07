using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Files.Application.Contracts;
using Files.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Files.Infrastructure.Services;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _blobContainerClient;
    
    public BlobService(BlobServiceClient blobServiceClient, IOptions<BlobOptions> blobOptions)
    {
        var containerName = blobOptions.Value.ContainerName;
        
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
    }
    
    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default)
    {
        var extension = Path.GetExtension(fileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        
        var blobClient = _blobContainerClient.GetBlobClient(uniqueFileName);

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType
        };
        
        await blobClient.UploadAsync(
            content: stream, 
            options: new BlobUploadOptions { HttpHeaders = blobHttpHeaders }, 
            cancellationToken: ct);
        
        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string blobName, CancellationToken ct = default)
    { 
        var blobClient = GetBlobClient(blobName);
        
        var download = await blobClient.DownloadStreamingAsync(cancellationToken: ct);
        
        return download.Value.Content;
    }

    public Task DeleteAsync(string blobName, CancellationToken ct = default)
    {
        return _blobContainerClient.GetBlobClient(blobName).DeleteIfExistsAsync(cancellationToken: ct);
    }
    
    private BlobClient GetBlobClient(string blobName) => 
        _blobContainerClient.GetBlobClient(blobName);
}