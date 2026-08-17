namespace BuildingBlocks.Interfaces;

public interface IUserContext
{
    Guid UserId { get; }
    string? Email { get; }
    IReadOnlyCollection<string> Roles { get; }
}
