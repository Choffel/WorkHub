using BuildingBlocks.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Users.Appliation.DTOs;
using Users.Appliation.Interfaces.Services;
using Users.Infrastructure.Identity;

namespace Users.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly RoleManager<RoleIdentity> _roleManager;
    private readonly ITokenService _tokenService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserManager<UserIdentity> userManager,
        RoleManager<RoleIdentity> roleManager,
        ITokenService tokenService,
        ILogger<UserService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<Result<string>> RegisterAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default)
    {
        var user = new UserIdentity
        {
            UserName = createUserDto.UserName,
            Email = createUserDto.Email,
        };

        var result = await _userManager.CreateAsync(user, createUserDto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Failure($"Error creating user: {errors}");
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        _logger.LogInformation("Email confirmation token for user {UserId}: {Token}", user.Id, token);

        return Result<string>.Success("user created successfully");
    }

    public async Task<Result<TokenResponseDto>> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<TokenResponseDto>.Failure("Invalid Email or Password");
        }

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (!isEmailConfirmed)
        {
            return Result<TokenResponseDto>.Failure("Email not confirmed");
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordCorrect)
        {
            return Result<TokenResponseDto>.Failure("Invalid Email or Password");
        }

        var userDto = new UserDto(user.Id, user.UserName ?? string.Empty, user.Email ?? string.Empty);

        var tokens = await _tokenService.GetTokens(userDto, cancellationToken);

        return Result<TokenResponseDto>.Success(new TokenResponseDto(tokens.AccessToken, tokens.RefreshToken));
    }

    public async Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<string>.Success("If this email address exists in the system, instructions have been sent to the email.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        _logger.LogInformation("Password reset token for user {UserId}: {Token}", user.Id, token);

        return Result<string>.Success("If this email address exists in the system, instructions have been sent to the email.");
    }

    public async Task<Result<string>> ResetPasswordAsync(ResetPasswordDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<string>.Failure($"{request.Email} - this email address is not registered");
        }

        var resetPassResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword!);

        if (!resetPassResult.Succeeded)
        {
            var errors = string.Join(", ", resetPassResult.Errors.Select(e => e.Description));
            return Result<string>.Failure(errors);
        }

        return Result<string>.Success("Password reset successfully");
    }

    public async Task<Result<string>> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<string>.Failure("Invalid email address");
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            return Result<string>.Failure("Invalid or expired confirmation token");
        }

        var roleExists = await _roleManager.RoleExistsAsync("User");
        if (!roleExists)
        {
            return Result<string>.Failure("Role 'User' does not exist");
        }

        await _userManager.AddToRoleAsync(user, "User");

        return Result<string>.Success("Email confirmed");
    }

    public async Task<Result<string>> UpdateUserAsync(UpdateUserDTO userDto, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
        if (user == null)
        {
            return Result<string>.Failure("User not found");
        }

        user.UserName = userDto.UserName;
        user.Email = userDto.Email;
        user.LastModified = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Failure($"Error updating user: {errors}");
        }

        return Result<string>.Success("User updated successfully");
    }
}
