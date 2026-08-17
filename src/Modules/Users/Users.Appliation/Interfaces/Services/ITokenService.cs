using System.Security.Claims;
using BuildingBlocks.Models;
using Users.Appliation.DTOs;

namespace Users.Appliation.Interfaces.Services;

public interface ITokenService
{
    Task<TokenResponseDto> GetTokens(UserDto userDto, CancellationToken cancellationToken = default);
    Task<Result<bool>> RevokeRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result<TokenResponseDto>> GetNewAccessTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}
