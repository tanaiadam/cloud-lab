using PhotoAlbum.Bll.Dtos;
using PhotoAlbum.Bll.Models;

namespace PhotoAlbum.Bll.Interfaces;

public interface IPhotoService
{
    Task<IEnumerable<PhotoDto>> GetPhotosAsync(PhotoFilterRequest request);

    Task<PhotoDto> UploadPhotoAsync(string fileName, Stream content);

    Task<bool> DeletePhotoAsync(Guid id);

    Task<PhotoDto?> RenamePhotoAsync(Guid id, string newName);
}
