using Files.Application.Contracts;
using Files.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Files.Infrastructure.Persistence.Repositories;

public class FileRepository : IFileRepository
{
    private readonly AppDbContext _dbContext;
    
    public FileRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Resume?> GetResumeByIdAsync(Guid id)
    {
        return _dbContext.Resumes.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
    }
    
    public async Task AddResumeAsync(Resume resume)
    {
        await _dbContext.Resumes.AddAsync(resume);
    }

    public async Task<Resume> UpdateResumeAsync(Resume resume)
    {
         _dbContext.Resumes.Update(resume);
         await _dbContext.SaveChangesAsync();
         return resume;
    }

    public Task DeleteResumeAsync(Resume resume, Guid userId)
    {
        _dbContext.Resumes.Remove(resume);
    }

    
    public Task SaveAsync()
    {
        return _dbContext.SaveChangesAsync();
    }
}