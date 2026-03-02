namespace PhotoAlbum.Dal.Entities;

public class Photo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    public string BlobUrl { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    // Navigational properties
    public User User { get; set; } = null!;
}
