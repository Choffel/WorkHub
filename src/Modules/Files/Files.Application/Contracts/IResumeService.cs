using Files.Application.DTOs;
using Files.Domain.Commands;
using Files.Domain.Models;

namespace Files.Application.Contracts;

public interface IResumeService
{
    Task<ResumeResponse> UploadResumeAsync(UploadFileCommand command, CancellationToken ct = default);
    
    Task<bool> DeleteResumeAsync(Guid userId,Guid resumeId, CancellationToken ct = default);
    
    Task<ResumeResponse> UpdateResumeAsync(Guid userId,Guid resumeId, UploadFileCommand command, CancellationToken ct = default);
}