using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BuildingBlocks.Interfaces;
using BuildingBlocks.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Users.Appliation.DTOs;
using Users.Appliation.Interfaces;
using Users.Appliation.Interfaces.Repositories;
using Users.Appliation.Interfaces.Services;
using Users.Domain.Entities;
using Users.Infrastructure.Identity;

namespace Users.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ITokenRepository _tokenRepository;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly ILogger<TokenService> _logger;
    private readonly IUserContext _userContext;


    public TokenService(IConfiguration configuration,
        ITokenRepository tokenRepository,
        UserManager<UserIdentity> userManager,
        ILogger<TokenService> logger,
        IUserContext userContext
        )
    {
        _configuration = configuration;
        _tokenRepository = tokenRepository;
        _userManager = userManager;
        _logger = logger;
        _userContext = userContext;
    }

    public async Task<TokenResponseDto> GetTokens(UserDto user, CancellationToken cancellationToken = default)
    {
        var accessToken = await CreateTokenAsync(user);
        var refreshToken = await CreateRefreshTokenAsync(user.Id);


        return new TokenResponseDto(accessToken, refreshToken);
    }

    private async Task<string> CreateRefreshTokenAsync(Guid userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = token,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        await _tokenRepository.AddRefreshTokenAsync(refreshToken);

        return token;
    }

    public async Task<Result<TokenResponseDto>> GetNewAccessTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var storedRefreshToken = await _tokenRepository.GetRefreshTokenAsync(request.RefreshToken);
        if (storedRefreshToken == null || storedRefreshToken.IsExpired)
        {
            return Result<TokenResponseDto>.Failure("Invalid or expired refresh token.");
        }

        storedRefreshToken.ExpiresAt = DateTime.UtcNow;

        

        var newRefreshToken = await CreateRefreshTokenAsync(storedRefreshToken.UserId);

        var user = await _userManager.FindByIdAsync(storedRefreshToken.UserId.ToString());
        if (user == null)
        {
            return Result<TokenResponseDto>.Failure("User not found.");
        }

        var userDto = new UserDto(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty);

        var accessToken = await CreateTokenAsync(userDto);

        return Result<TokenResponseDto>.Success(new TokenResponseDto(accessToken, newRefreshToken));
    }

    public async Task<Result<bool>> RevokeRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _tokenRepository.GetRefreshTokenAsync(request.RefreshToken);
        if (refreshToken == null || refreshToken.UserId != _userContext.UserId || refreshToken.IsExpired)
        {
            return Result<bool>.Failure("Invalid refresh token.");
        }

        refreshToken.ExpiresAt = DateTime.UtcNow;
        

        _logger.LogInformation("Refresh token revoked for user {UserId}", _userContext.UserId);

        return Result<bool>.Success(true);
    }

    private async Task<string> CreateTokenAsync(UserDto user)
    {
        int expirationMinutes = int.Parse(_configuration["JwtTokenSettings:ExpirationMinutes"] ?? "60");
        var expiration = DateTime.UtcNow.AddHours(10).AddMinutes(expirationMinutes);
        var token = CreateJwtToken(
            await CreateClaimsAsync(user),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) =>
        new(
            _configuration["JwtTokenSettings:ValidIssuer"],
            _configuration["JwtTokenSettings:ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private async Task<List<Claim>> CreateClaimsAsync(UserDto user)
    {
        var jwtSub = _configuration["JwtTokenSettings:JwtRegisteredClaimNamesSub"];

        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var userEntity = await _userManager.FindByIdAsync(user.Id.ToString());

        if (userEntity != null)
        {
            var roles = await _userManager.GetRolesAsync(userEntity);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        return claims;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var symmetricSecurityKey = _configuration["JwtTokenSettings:SymmetricSecurityKey"];

        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)),
            SecurityAlgorithms.HmacSha256
        );
    }
}