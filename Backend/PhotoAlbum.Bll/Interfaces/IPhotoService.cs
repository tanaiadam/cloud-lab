using PhotoAlbum.Bll.Dtos;
using PhotoAlbum.Bll.Models;

namespace PhotoAlbum.Bll.Interfaces;

public interface IPhotoService
{
    Task<IEnumerable<PhotoDto>> GetPhotosAsync(PhotoFilterRequest request, Guid userId);

    Task<PhotoDto> UploadPhotoAsync(string fileName, Stream content, Guid userId);

    Task<bool> DeletePhotoAsync(Guid id, Guid userId);

    Task<PhotoDto?> RenamePhotoAsync(Guid id, string newName, Guid userId);
}
