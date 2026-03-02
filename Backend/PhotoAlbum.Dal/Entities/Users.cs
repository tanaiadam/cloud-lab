namespace PhotoAlbum.Dal.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    // Navigational properties
    public ICollection<Photo> Photos { get; set; } = null!;
}
