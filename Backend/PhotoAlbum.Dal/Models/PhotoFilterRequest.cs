namespace PhotoAlbum.Dal.Models;

public class PhotoFilterRequest
{
    public Guid UserId { get; set; }

    public string? Name { get; set; }

    public DateTime? UploadDateAfter { get; set; }

    public DateTime? UploadDateBefore { get; set; }
}
