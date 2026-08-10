using Files.Application.DTOs;
using Files.Domain.Commands;
using Files.Domain.Models;

namespace Files.Application.Contracts;

public interface IResumeService
{
    Task<ResumeResponse> UploadResumeAsync(UploadFileCommand command, CancellationToken ct = default);
    
}