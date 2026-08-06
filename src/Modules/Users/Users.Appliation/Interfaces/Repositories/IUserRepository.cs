using Users.Domain.Entities;

namespace Users.Appliation.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email);
    Task<User?> FindByIdAsync(Guid id);
    Task<User> CreateAsync(User user, string password);
    Task UpdateAsync(User user);
    Task<bool> CheckPasswordAsync(User user, string password);
    Task ConfirmEmailAsync(User user);
    Task ResetPasswordAsync(User user, string token, string newPassword);
    Task AddToRoleAsync(User user, string role);
    Task<bool> RoleExistsAsync(string role);
}
