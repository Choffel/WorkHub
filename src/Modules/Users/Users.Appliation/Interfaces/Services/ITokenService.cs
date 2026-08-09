using System.Security.Claims;
using BuildingBlocks.Models;
using Users.Appliation.DTOs;

namespace Users.Appliation.Interfaces.Services;

public interface ITokenService
{
    Task<TokenResponseDto> GetTokens(UserDto userDto);
    Task<Result<bool>> RevokeRefreshTokenAsync(RefreshTokenRequest request);
    Task<Result<TokenResponseDto>> GetNewAccessTokenAsync(RefreshTokenRequest request);
}
