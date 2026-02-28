using PhotoAlbum.Dal.Entities;

namespace PhotoAlbum.Dal.Repositories;

public interface IPhotoRepository
{
    Task<IEnumerable<Photo>> GetAllAsync();

    Task<Photo?> GetByIdAsync(Guid id);

    Task AddAsync(Photo photo);

    Task DeleteAsync(Photo photo);
}
