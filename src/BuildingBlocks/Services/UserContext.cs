using System.Security.Claims;
using BuildingBlocks.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Services;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    private Guid? _userId;
    private List<string>? _roles;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    
    public Guid UserId => _userId ??= GetUserId();

    public string? Email => User?.FindFirstValue(ClaimTypes.Email);

    public IReadOnlyCollection<string> Roles => _roles ??= User?.FindAll(ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList() ?? [];

    private Guid GetUserId()
    {
        var id = User?.FindFirstValue("userId");

        return Guid.TryParse(id, out var parsed) ? parsed : Guid.Empty;
    }
}