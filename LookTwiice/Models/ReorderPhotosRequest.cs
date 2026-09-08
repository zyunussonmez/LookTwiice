namespace LookTwiice.Models.ViewModels;

public class ReorderPhotosRequest
{
    public int GalleryId { get; set; }

    public List<int> PhotoIds { get; set; } = new();
}