using Files.Application.Contracts;
using Files.Application.DTOs;
using Files.Domain.Commands;
using Files.Domain.Models;

namespace Files.Application.Services;

public class ResumeService : IResumeService
{
    private readonly IBlobService _blobService;
    private readonly IFileRepository _fileRepository;

    public ResumeService(IBlobService blobService, IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
        _blobService = blobService;
    }


    public async Task<ResumeResponse> UploadResumeAsync(UploadFileCommand command, CancellationToken ct = default)
    {
        var validCommand = UploadFileCommand.Create(
            command.UserId,
            command.Stream,
            command.FileName,
            command.ContentType,
            command.Length,
            command.UpdatedAt,
            command.BlobName
        );

        var uploadResult = await _blobService.UploadAsync(
            validCommand.Stream,
            validCommand.FileName,
            validCommand.ContentType,
            ct
        );

        var resume = Resume.Create(
            validCommand.UserId,
            validCommand.FileName,
            uploadResult.BlobName,
            validCommand.ContentType,
            validCommand.Length
        );

        await _fileRepository.AddResumeAsync(resume);

        return new ResumeResponse(
            resume.UserId,
            resume.Id,
            resume.FileName,
            resume.ContentType,
            resume.Size,
            resume.CreatedAt
        );
    }
    
    public async Task<bool> DeleteResumeAsync(Guid userId, Guid resumeId, CancellationToken ct = default)
    {
        var resume = await _fileRepository.GetResumeByIdAsync(resumeId);

        if (resume.UserId != userId)
        {
            throw new Exception("You are not authorized to delete this resume.");
        }
        
        resume.Delete();
        
        await _fileRepository.DeleteResumeAsync(resume);

        return true;
    }

    public async Task<ResumeResponse> UpdateResumeAsync(
        Guid userId, 
        Guid resumeId, 
        UploadFileCommand command, 
        CancellationToken ct = default)
    {
        
        var resume = await _fileRepository.GetResumeByIdAsync(resumeId);

        
        if (resume == null || resume.IsDeleted)
        {
            throw new Exception("Resume deleted or null");
        }

       
        if (resume.UserId != userId)
        {
            throw new Exception("It's not your resume.");
        }
        
        resume.UpdateFile(command.FileName,command.Length,command.UpdatedAt, command.ContentType,command.BlobName);
        
        await _blobService.UploadAsync(command.Stream,command.FileName,command.ContentType,ct);

        return new ResumeResponse(
            resume.UserId,
            resume.Id,
            resume.FileName,
            resume.ContentType,
            resume.Size,
            resume.CreatedAt);

    }
}