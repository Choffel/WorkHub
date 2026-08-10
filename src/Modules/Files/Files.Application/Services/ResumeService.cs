using Files.Application.Contracts;
using Files.Application.DTOs;
using Files.Domain.Commands;
using Files.Domain.Models;

namespace Files.Application.Services;

public class ResumeService : IResumeService
{
    private readonly IBlobService _blobService;

    public ResumeService(IBlobService blobService)
    {
        _blobService = blobService;
    }


    public async Task<ResumeResponse> UploadResumeAsync(UploadFileCommand command, CancellationToken ct = default)
    {
        UploadFileCommand.Create(command.Stream, command.FileName, command.ContentType, command.Length);
        
        var uploadResult = await _blobService.UploadAsync(command.Stream, command.FileName, command.ContentType, ct);
        
        var resume = new Resume
        {
            Id = Guid.NewGuid(),
            FileName = command.FileName,
            BlobName = uploadResult.BlobName,
            ContentType = command.ContentType,
            Size = command.Length,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
         
        return new ResumeResponse(command.FileName, command.ContentType, command.Stream, command.Length);
    }
}