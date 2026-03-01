namespace PhotoAlbum.Bll.Dtos;

public class PhotoDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime UploadDate { get; set; }

    public string BlobUrl { get; set; } = string.Empty;
}
