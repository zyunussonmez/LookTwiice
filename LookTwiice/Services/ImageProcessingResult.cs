namespace LookTwiice.Services;

public class ImageProcessingResult
{
    public string OriginalUrl { get; set; } = string.Empty;
    public string WebUrl { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;

    public string OriginalFileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
}