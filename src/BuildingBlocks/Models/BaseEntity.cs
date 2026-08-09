namespace BuildingBlocks.Models;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}