namespace LookTwiice.Models;

public class ContactInquiry : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }

    public string ServiceType { get; set; } = string.Empty;

    public DateTime? EventDate { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Budget { get; set; }

    public string Message { get; set; } = string.Empty;

    public InquiryStatus Status { get; set; } = InquiryStatus.New;

    public string? PhotographerNotes { get; set; }
}