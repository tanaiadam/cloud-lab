using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using PhotoAlbum.Bll.Dtos;
using PhotoAlbum.Bll.Interfaces;
using PhotoAlbum.Bll.Models;
using PhotoAlbum.Dal.Entities;
using PhotoAlbum.Dal.Interfaces;

namespace PhotoAlbum.Bll.Services;

public class PhotoService : IPhotoService
{
    private readonly IPhotoRepository _repository;
    private readonly BlobContainerClient _blobContainerClient;

    public PhotoService(IPhotoRepository repository, BlobServiceClient blobServiceClient)
    {
        _repository = repository;
        _blobContainerClient = blobServiceClient.GetBlobContainerClient("photos");
    }

    public async Task<IEnumerable<PhotoDto>> GetPhotosAsync(PhotoFilterRequest request)
    {
        var photos = await _repository.GetFilteredPhotosAsync(new Dal.Models.PhotoFilterRequest()
        {
            Name = request.Name,
            UploadDateBefore = request.UploadDateBefore,
            UploadDateAfter = request.UploadDateAfter
        });

        return photos.Select(p => new PhotoDto
        {
            Id = p.Id,
            Name = p.Name,
            UploadDate = p.UploadDate,
            BlobUrl = p.BlobUrl
        });
    }
    public async Task<PhotoDto> UploadPhotoAsync(string fileName, Stream content)
    {
        var photoId = Guid.NewGuid();
        var safeFileName = fileName.Length > 40 ? fileName[..40] : fileName;
        var blobName = $"{photoId}_{safeFileName}";

        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        var httpHeaders = new BlobHttpHeaders { ContentType = "image/jpeg" };
        await blobClient.UploadAsync(content, new BlobUploadOptions { HttpHeaders = httpHeaders });

        var photo = new Photo
        {
            Id = photoId,
            Name = safeFileName,
            UploadDate = DateTime.UtcNow,
            BlobUrl = blobClient.Uri.ToString()
        };

        await _repository.AddAsync(photo);

        return new PhotoDto
        {
            Id = photo.Id,
            Name = photo.Name,
            UploadDate = photo.UploadDate,
            BlobUrl = photo.BlobUrl
        };
    }

    public async Task<bool> DeletePhotoAsync(Guid id)
    {
        var photo = await _repository.GetByIdAsync(id);
        if (photo == null) return false;

        var blobName = photo.BlobUrl.Split('/').Last();
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();

        await _repository.DeleteAsync(photo);
        return true;
    }

    public async Task<PhotoDto?> RenamePhotoAsync(Guid id, string newName)
    {
        var photo = await _repository.GetByIdAsync(id);
        if (photo == null) return null;

        photo.Name = newName.Length > 40 ? newName[..40] : newName;

        await _repository.UpdateAsync(photo);

        return new PhotoDto
        {
            Id = photo.Id,
            Name = photo.Name,
            UploadDate = photo.UploadDate,
            BlobUrl = photo.BlobUrl
        };
    }
}
