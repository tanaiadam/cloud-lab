using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Dal.Entities;

namespace PhotoAlbum.Dal;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Photo> Photos { get; set; }
}
