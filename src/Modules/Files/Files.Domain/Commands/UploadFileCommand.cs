using System.Windows.Input;

namespace Files.Domain.Commands;

public record UploadFileCommand(Guid UserId,Stream Stream, string FileName, string ContentType, long Length, DateTime UpdatedAt, string BlobName)
{
    public static UploadFileCommand Create(Guid UserId,Stream Stream, string FileName, string ContentType, long Length, DateTime UpdatedAt, string BlobName)
    {
        var allowedExtensions = new[] { ".pdf", ".docx" };
        var extension = Path.GetExtension(FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new ArgumentException($"Unsupported file extension: {extension}");

        if (Length > 10 * 1024 * 1024)
            throw new ArgumentException("File size exceeds 10 MB limit.");

        return new UploadFileCommand(UserId,Stream, FileName, ContentType, Length, UpdatedAt,  BlobName);
    }
}