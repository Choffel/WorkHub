using BuildingBlocks.Models;
using Users.Appliation.DTOs;

namespace Users.Appliation.Interfaces.Services;

public interface IUserService
{
    Task<Result<TokenResponseDto>> AuthenticateAsync(AuthRequest request);
    Task<Result<string>> RegisterAsync(CreateUserDto user);
    Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<Result<string>> ResetPasswordAsync(ResetPasswordDto request);
    Task<Result<string>> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result<string>> UpdateUserAsync(UpdateUserDTO userDto);
}