namespace LookTwiice.Models;

public class Photo : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string OriginalFileName { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public string WebUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;
    public long FileSize { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public int DisplayOrder { get; set; }
    public bool IsFeatured { get; set; }

    public int GalleryId { get; set; }
    public Gallery? Gallery { get; set; }
}