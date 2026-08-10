namespace Files.Domain.Commands;

public record UploadFileCommand(Stream Stream, string FileName, string ContentType, long Length)
{
    public static UploadFileCommand Create(Stream Stream, string FileName, string ContentType, long Length)
    {
        var allowedExtensions = new[] { ".pdf", ".docx" };
        var extension = Path.GetExtension(FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new ArgumentException($"Unsupported file extension: {extension}");

        if (Length > 10 * 1024 * 1024)
            throw new ArgumentException("File size exceeds 10 MB limit.");

        return new UploadFileCommand(Stream, FileName, ContentType, Length);
    }
}