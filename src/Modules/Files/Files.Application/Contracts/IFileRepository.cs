using Files.Domain.Models;

namespace Files.Application.Contracts;

public interface IFileRepository
{
    Task<Resume?> GetResumeByIdAsync(Guid resumeId);
    Task AddResumeAsync(Resume resume);
    Task<Resume> UpdateResumeAsync(Resume resume);
    
    Task DeleteResumeAsync(Resume resume);
    Task SaveAsync();
}