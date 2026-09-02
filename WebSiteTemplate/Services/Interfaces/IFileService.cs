namespace WebSiteTemplate.Services.Interfaces
{
    public class IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        void DeleteFile(string filePath);
    }
}
