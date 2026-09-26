using System.ComponentModel.DataAnnotations;

namespace LookTwiice.ViewModels;

public class ContactInquiryViewModel
{
    [Required(ErrorMessage = "Please enter your name.")]
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Please select a service.")]
    [Display(Name = "Service")]
    public string ServiceType { get; set; } = string.Empty;

    [Display(Name = "Event Date")]
    [DataType(DataType.Date)]
    public DateTime? EventDate { get; set; }

    [Required(ErrorMessage = "Please select a country.")]
    [Display(Name = "Country")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a city.")]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    [Display(Name = "Budget")]
    public string? Budget { get; set; }

    [Required(ErrorMessage = "Please tell us a little about your story.")]
    [Display(Name = "Message")]
    public string Message { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please accept the communication consent.")]
    public bool Consent { get; set; }
}