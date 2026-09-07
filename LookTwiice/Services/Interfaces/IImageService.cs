using LookTwiice.Services;

namespace LookTwiice.Services.Interfaces;

public interface IImageService
{
    Task<ImageProcessingResult> ProcessAsync(
        IFormFile file,
        string folder);
}