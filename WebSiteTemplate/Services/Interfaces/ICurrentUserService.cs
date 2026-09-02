namespace WebSiteTemplate.Services.Interfaces
{
    public class ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}
