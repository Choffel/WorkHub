using Microsoft.EntityFrameworkCore;
using Users.Appliation.Interfaces.Repositories;
using Users.Domain.Entities;
using Users.Infrastructure.Data;

namespace Users.Infrastructure.Repositories;

public class TokenRepository : ITokenRepository
{
    
    private readonly ApplicationDbContext _dbContext;
    public TokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
        return _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Update(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<RefreshToken>> GetActiveRefreshTokensAsync(Guid userId,
        CancellationToken ct = default)
    {
        return await _dbContext.RefreshTokens
            .AsNoTracking()
            .Where(rf => rf.UserId == userId && rf.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(ct);
    }
}