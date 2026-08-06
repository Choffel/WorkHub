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
    public async Task<Result<string>> Register([FromBody] CreateUserDto createUserDto)    
    {                                                                                     
        var result = await _userService.RegisterAsync(createUserDto);
        return result;
    }

    [HttpPost("login")]
    public async Task<Result<TokenResponseDto>> LoginAsync([FromBody] AuthRequest request) 
    {
        var result = await _userService.AuthenticateAsync(request);
        return result;
    }

    [HttpPost("ForgotPassword")]
    public async Task<Result<string>> ForgotPasswordAsync([FromBody ]ForgotPasswordRequest request)
    {
        var result = await _userService.ForgotPasswordAsync(request);
        return result;
    }

    [HttpPost("ResetPassword")] // TODO разобраться с хттп методами
    public async Task<Result<string>> ResetPasswordAsync([FromBody] ResetPasswordDto request)
    {
        var result = await _userService.ResetPasswordAsync(request);
        return result;
    }

    [HttpPost("ConfirmEmail")]
    public async Task<Result<string>> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request)
    {
        var result = await _userService.ConfirmEmailAsync(request);
        return result;
    }
}