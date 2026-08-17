using BuildingBlocks.Models;
using Users.Appliation.DTOs;

namespace Users.Appliation.Interfaces.Services;

public interface IUserService
{
    Task<Result<TokenResponseDto>> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> RegisterAsync(CreateUserDto user, CancellationToken cancellationToken = default);
    Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> ResetPasswordAsync(ResetPasswordDto request, CancellationToken cancellationToken = default);
    Task<Result<string>> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default);
    Task<Result<string>> UpdateUserAsync(UpdateUserDTO userDto, CancellationToken cancellationToken = default);
}