using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Dal.Entities;

namespace PhotoAlbum.Dal.Repositories;

public class PhotoRepository(AppDbContext context) : IPhotoRepository
{
    public async Task<IEnumerable<Photo>> GetAllAsync() =>
        await context.Photos.ToListAsync();

    public async Task<Photo?> GetByIdAsync(Guid id) =>
        await context.Photos.FindAsync(id);

    public async Task AddAsync(Photo photo)
    {
        context.Photos.Add(photo);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Photo photo)
    {
        context.Photos.Remove(photo);
        await context.SaveChangesAsync();
    }
}
