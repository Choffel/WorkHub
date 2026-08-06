using Microsoft.AspNetCore.Identity;
using Users.Domain.Entities;

namespace Users.Infrastructure.Identity;

public class UserIdentity: IdentityUser<Guid>, IUser
{ 
    
    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModified { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<IdentityUserRole<Guid>>? UserRoles { get; set; }
}