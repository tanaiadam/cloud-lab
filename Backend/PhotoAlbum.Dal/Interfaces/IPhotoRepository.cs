using PhotoAlbum.Dal.Entities;
using PhotoAlbum.Dal.Models;

namespace PhotoAlbum.Dal.Interfaces;

public interface IPhotoRepository
{
    Task<IEnumerable<Photo>> GetFilteredPhotosAsync(PhotoFilterRequest request);

    Task<Photo?> GetByIdAsync(Guid id);

    Task AddAsync(Photo photo);

    Task DeleteAsync(Photo photo);

    Task UpdateAsync(Photo photo);
}
