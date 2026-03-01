using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.Dal.Entities;

public class Photo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;

    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    public string BlobUrl { get; set; } = string.Empty;
}
