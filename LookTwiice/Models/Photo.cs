namespace LookTwiice.Models;

public class Photo : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }

    public int GalleryId { get; set; }
    public Gallery Gallery { get; set; } = null!;
}