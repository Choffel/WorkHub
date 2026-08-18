using BuildingBlocks.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Appliation.DTOs;
using Users.Appliation.Interfaces.Services;

namespace Host.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TokensController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokensController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("RevokeRefreshToken")]
    [Authorize]
    public async Task<Result<bool>> RevokeRefreshToken(CancellationToken cancellationToken)
    {
        var result = await _tokenService.RevokeRefreshTokenAsync(cancellationToken);
        return result;
    }
    
    [AllowAnonymous]
    [HttpPost("NewAccessToken")]
    public async Task<Result<TokenResponseDto>> GetNewAccessToken(CancellationToken cancellationToken)
    {
        var result = await _tokenService.GetNewAccessTokenAsync(cancellationToken);
        return result;
    }

    [Authorize]
    [HttpGet("ActiveSessions")]
    public async Task<Result<IEnumerable<ActiveSessionDto>>> GetActiveSessionsAsync(CancellationToken ct = default)
    {
        var result = await _tokenService.GetActiveSessionsAsync(ct);
        return result;
    }
}