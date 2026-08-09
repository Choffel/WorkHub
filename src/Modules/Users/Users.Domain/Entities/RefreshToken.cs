using BuildingBlocks.Models;

namespace Users.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}