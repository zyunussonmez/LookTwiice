using LookTwiice.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace LookTwiice.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

    private const long MaxFileSize = 20 * 1024 * 1024;
    private const int MaxImageSize = 2000;
    private const int ThumbnailSize = 400;

    private static readonly string[] AllowedMimeTypes =
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<ImageProcessingResult> ProcessAsync(
        IFormFile file,
        string folder)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No image file was provided.");
        }

        if (file.Length > MaxFileSize)
        {
            throw new ArgumentException("Image size cannot exceed 20 MB.");
        }

        if (!AllowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            throw new ArgumentException("Only JPEG, PNG and WebP images are allowed.");
        }

        await using var inputStream = file.OpenReadStream();

        using var image = await Image.LoadAsync(inputStream);

        var width = image.Width;
        var height = image.Height;

        var fileName = Guid.NewGuid().ToString("N");

        var originalFolder = Path.Combine(
            _environment.WebRootPath,
            folder,
            "original");

        var webFolder = Path.Combine(
            _environment.WebRootPath,
            folder,
            "web");

        var thumbnailFolder = Path.Combine(
            _environment.WebRootPath,
            folder,
            "thumbnail");

        Directory.CreateDirectory(originalFolder);
        Directory.CreateDirectory(webFolder);
        Directory.CreateDirectory(thumbnailFolder);

        var originalExtension = GetExtension(file.ContentType);

        var originalFileName = $"{fileName}{originalExtension}";
        var webFileName = $"{fileName}.webp";
        var thumbnailFileName = $"{fileName}.webp";

        var originalPath = Path.Combine(
            originalFolder,
            originalFileName);

        var webPath = Path.Combine(
            webFolder,
            webFileName);

        var thumbnailPath = Path.Combine(
            thumbnailFolder,
            thumbnailFileName);

        await using (var originalStream = File.Create(originalPath))
        {
            await file.CopyToAsync(originalStream);
        }

        using var webImage = image.Clone();

        ResizeImage(webImage, MaxImageSize);

        await webImage.SaveAsWebpAsync(
            webPath,
            new WebpEncoder
            {
                Quality = 82
            });

        using var thumbnailImage = image.Clone();

        ResizeImage(thumbnailImage, ThumbnailSize);

        await thumbnailImage.SaveAsWebpAsync(
            thumbnailPath,
            new WebpEncoder
            {
                Quality = 78
            });

        return new ImageProcessingResult
        {
            OriginalUrl = $"/{folder}/original/{originalFileName}",
            WebUrl = $"/{folder}/web/{webFileName}",
            ThumbnailUrl = $"/{folder}/thumbnail/{thumbnailFileName}",

            OriginalFileName = file.FileName,
            MimeType = file.ContentType,
            FileSize = file.Length,

            Width = width,
            Height = height
        };
    }

    private static void ResizeImage(
        Image image,
        int maxSize)
    {
        if (image.Width <= maxSize &&
            image.Height <= maxSize)
        {
            return;
        }

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(maxSize, maxSize)
        }));
    }

    private static string GetExtension(string contentType)
    {
        return contentType.ToLowerInvariant() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => throw new ArgumentException("Unsupported image type.")
        };
    }
}