namespace Files.Domain.Models;

public class Resume
{
    public Guid Id { get; set; }
    
    //public User user 
    
    public string FileName { get; set; }
    
    public string BlobName { get; set; }
    
    public string ContentType { get; set; }
    
    public long Size { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsDeleted { get; set; }
    
    
    public void Delete()
    {
        IsDeleted = true;
    }
}