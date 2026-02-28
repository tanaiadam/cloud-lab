using PhotoAlbum.Bll.Interfaces;
using PhotoAlbum.Dal.Entities;
using PhotoAlbum.Dal.Repositories;

namespace PhotoAlbum.Bll.Services;

public class PhotoService(IPhotoRepository repository) : IPhotoService
{
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

    public async Task<IEnumerable<Photo>> GetPhotosAsync(string sortBy)
    {
        var photos = await repository.GetAllAsync();

        return sortBy.ToLower() switch
        {
            "date" => photos.OrderBy(p => p.UploadDate),
            _ => photos.OrderBy(p => p.Name)
        };
    }

    public async Task<Photo> UploadPhotoAsync(string fileName, Stream content)
    {
        if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);

        var photo = new Photo
        {
            Id = Guid.NewGuid(),
            Name = fileName.Length > 40 ? fileName[..40] : fileName,
            UploadDate = DateTime.UtcNow
        };

        await repository.AddAsync(photo);

        var filePath = Path.Combine(_storagePath, $"{photo.Id}_{photo.Name}");
        using var fileStream = new FileStream(filePath, FileMode.Create);
        await content.CopyToAsync(fileStream);

        return photo;
    }

    public async Task<bool> DeletePhotoAsync(Guid id)
    {
        var photo = await repository.GetByIdAsync(id);
        if (photo == null) return false;

        await repository.DeleteAsync(photo);

        var filePath = Path.Combine(_storagePath, $"{photo.Id}_{photo.Name}");
        if (File.Exists(filePath)) File.Delete(filePath);

        return true;
    }
}
