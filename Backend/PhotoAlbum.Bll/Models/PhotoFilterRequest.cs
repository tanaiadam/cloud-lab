namespace PhotoAlbum.Bll.Models;

public class PhotoFilterRequest
{
    public string? Name { get; set; }
    public DateTime? UploadDateAfter { get; set; }
    public DateTime? UploadDateBefore { get; set; }
}
