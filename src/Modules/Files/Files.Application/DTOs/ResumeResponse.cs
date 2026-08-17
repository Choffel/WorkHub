namespace Files.Application.DTOs;

public record ResumeResponse(
    Guid UserId,
    Guid ResumeId,
    string FileName,
    string ContentType,
    long Length,
    DateTime CreatedAt
    );