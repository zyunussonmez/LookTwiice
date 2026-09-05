namespace LookTwiice.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<Gallery> Galleries { get; set; } = new List<Gallery>();
}