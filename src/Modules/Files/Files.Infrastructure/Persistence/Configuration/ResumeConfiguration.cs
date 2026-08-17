using BuildingBlocks.Models;
using Files.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Files.Infrastructure.Persistence.Configuration;

public class ResumeConfiguration : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.FileName).IsRequired();
        builder.Property(r => r.BlobName).IsRequired();
        builder.Property(r => r.ContentType).IsRequired();

        builder.HasIndex(r => r.UserId).IsUnique(); 

        builder.HasOne<BaseEntity>()
            .WithOne()
            .HasForeignKey<Resume>(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}