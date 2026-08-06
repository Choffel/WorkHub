using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Infrastructure.Identity;

namespace Users.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<UserIdentity, IdentityRole<Guid>, Guid>
{
    

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*modelBuilder.Entity<IdentityUserRole<Guid>>(ur =>
        {
            ur.HasKey(r => new { r.UserId, r.RoleId });  // TODO !!!
            ur.HasOne<RoleIdentity>().WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId);
        });
        
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).IsRequired();

            entity.HasOne<User>()
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });*/
        
    }
}