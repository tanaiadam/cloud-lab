using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Dal.Entities;

namespace PhotoAlbum.Dal;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.PasswordHash)
                .IsRequired();
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(40);


            entity.HasOne(p => p.User)
                .WithMany(u => u.Photos)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
