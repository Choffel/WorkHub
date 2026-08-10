namespace Files.Application.DTOs;

public record ResumeResponse(
    string FileName,
    string ContentType,
    Stream FileStream,
    long Lenght
    );