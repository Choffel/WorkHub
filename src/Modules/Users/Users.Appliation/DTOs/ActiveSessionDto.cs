namespace Users.Appliation.DTOs;

public record ActiveSessionDto(
    Guid TokenId,
    DateTime CreatedAt,
    bool IsCurrentSession
    );