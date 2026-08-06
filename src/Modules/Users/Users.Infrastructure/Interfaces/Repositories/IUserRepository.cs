using Users.Infrastructure.Identity;

namespace Users.Infrastructure.Interfaces.Repositories;

public interface IUserRepository
{
    Task<UserIdentity?> FindByEmailAsync(string email);
    Task<UserIdentity?> FindByIdAsync(Guid id);
    Task<UserIdentity> CreateAsync(UserIdentity user, string password);
    Task UpdateAsync(UserIdentity user);
    Task<bool> CheckPasswordAsync(UserIdentity user, string password);
    Task ConfirmEmailAsync(UserIdentity user);
    Task ResetPasswordAsync(UserIdentity user, string token, string newPassword);
    Task AddToRoleAsync(UserIdentity user, string role);
    Task<bool> RoleExistsAsync(string role);
}