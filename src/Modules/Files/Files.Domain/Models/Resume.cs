namespace Files.Domain.Models;

public class Resume
{
    public Guid Id { get;  private set; }
    
    public Guid UserId { get; private set; }    
    
    public string FileName { get; private set; }
    
    public string BlobName { get; private set; }
    
    public string ContentType { get; private set; }
    public long Size { get;  private set; }
    
    public DateTime UpdatedAt { get;  private set; }
    
    public DateTime CreatedAt { get;  private set; }
    
    public bool IsDeleted { get;  private set; }

    public Resume(Guid userId, string fileName, string blobName, string contentType, long size)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        FileName = fileName;
        BlobName = blobName;
        ContentType = contentType;
        Size = size;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public static Resume Create(Guid userId, string fileName, string blobName, string contentType, long size)
    {
        return new Resume(userId, fileName, blobName, contentType, size);
    }
    
    
    public void Delete()
    {
        UpdatedAt =  DateTime.UtcNow;
        IsDeleted = true;
    }
    
    public void UpdateFile(string filename,  long size, DateTime updatedAt, string contentType, string blobName)
    {
        FileName = filename;
        Size = size;
        UpdatedAt = updatedAt;
        ContentType = contentType;
        BlobName = blobName;
    }
}