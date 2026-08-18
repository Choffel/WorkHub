using BuildingBlocks.Models;
using Microsoft.AspNetCore.Mvc;
using Users.Appliation.DTOs;
using Users.Appliation.Interfaces.Services;

namespace Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<Result<string>> Register([FromBody] CreateUserDto createUserDto, CancellationToken cancellationToken)    
    {
        var result = await _userService.RegisterAsync(createUserDto, cancellationToken);
        return result;
    }

    [HttpPost("login")]
    public async Task<Result<TokenResponseDto>> LoginAsync([FromBody] AuthRequest request, CancellationToken cancellationToken) 
    {
        var result = await _userService.AuthenticateAsync(request, cancellationToken);
        return result;
    }

    [HttpPost("ForgotPassword")]
    public async Task<Result<string>> ForgotPasswordAsync([FromBody ]ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.ForgotPasswordAsync(request, cancellationToken);
        return result;
    }

    [HttpPost("ResetPassword")]
    public async Task<Result<string>> ResetPasswordAsync([FromBody] ResetPasswordDto request, CancellationToken cancellationToken)
    {
        var result = await _userService.ResetPasswordAsync(request, cancellationToken);
        return result;
    }

    [HttpPost("ConfirmEmail")]
    public async Task<Result<string>> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.ConfirmEmailAsync(request, cancellationToken);
        return result;
    }
}