using Users.Domain.Entities;

namespace Users.Appliation.Interfaces.Repositories;

public interface ITokenRepository
{
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
}
