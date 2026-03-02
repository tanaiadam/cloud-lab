using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Dal.Entities;
using PhotoAlbum.Dal.Interfaces;
using PhotoAlbum.Dal.Models;

namespace PhotoAlbum.Dal.Repositories;

public class PhotoRepository : IPhotoRepository
{
    private readonly AppDbContext _context;

    public PhotoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Photo>> GetFilteredPhotosAsync(PhotoFilterRequest request)
    {
        var query = _context.Photos.Where(p => p.UserId == request.UserId);

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(p => p.Name.Contains(request.Name));
        }

        if (request.UploadDateAfter.HasValue)
        {
            query = query.Where(p => p.UploadDate >= request.UploadDateAfter.Value);
        }

        if (request.UploadDateBefore.HasValue)
        {
            query = query.Where(p => p.UploadDate < request.UploadDateBefore.Value.AddDays(1));
        }

        return await query
            .OrderByDescending(p => p.UploadDate)
            .ToListAsync();
    }

    public async Task<Photo?> GetByIdAsync(Guid id, Guid userId) =>
        await _context.Photos.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

    public async Task AddAsync(Photo photo)
    {
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Photo photo)
    {
        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Photo photo)
    {
        _context.Photos.Update(photo);
        await _context.SaveChangesAsync();
    }
}
