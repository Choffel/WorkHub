namespace Files.Infrastructure.Options;

public class BlobOptions
{
    public const string BlobContainerName = "resumes";

    public string ContainerName { get; set; } = string.Empty;
}