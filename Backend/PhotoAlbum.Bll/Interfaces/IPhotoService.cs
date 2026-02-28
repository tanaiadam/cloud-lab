using PhotoAlbum.Dal.Entities;

namespace PhotoAlbum.Bll.Interfaces;

public interface IPhotoService
{
    Task<IEnumerable<Photo>> GetPhotosAsync(string sortBy);

    Task<Photo> UploadPhotoAsync(string fileName, Stream content);

    Task<bool> DeletePhotoAsync(Guid id);
}
