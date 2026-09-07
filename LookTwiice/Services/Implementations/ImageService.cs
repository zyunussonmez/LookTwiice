using LookTwiice.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace LookTwiice.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

    private const long MaxFileSize = 20 * 1024 * 1024;
    private const int MaxImageSize = 4000;
    private const int ThumbnailSize = 500;

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
        Console.WriteLine("STEP 1 - ProcessAsync başladı");

        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No image file was provided.");
        }

        Console.WriteLine($"STEP 2 - File geldi: {file.FileName}");
        Console.WriteLine($"STEP 3 - Size: {file.Length}");
        Console.WriteLine($"STEP 4 - ContentType: {file.ContentType}");

        if (file.Length > MaxFileSize)
        {
            throw new ArgumentException("Image size cannot exceed 20 MB.");
        }

        if (!AllowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
        {
            throw new ArgumentException("Only JPEG, PNG and WebP images are allowed.");
        }

        Console.WriteLine("STEP 5 - Validation tamam");

        await using var inputStream = file.OpenReadStream();

        Console.WriteLine("STEP 6 - Stream açıldı");

        using var image = await Image.LoadAsync(inputStream);

        Console.WriteLine("STEP 7 - ImageSharp image yükledi");

        var width = image.Width;
        var height = image.Height;

        Console.WriteLine($"STEP 8 - Dimensions: {width}x{height}");

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

        Console.WriteLine("STEP 9 - Klasörler oluşturuldu");

        var originalExtension = GetExtension(file.ContentType);

        var originalFileName = $"{fileName}{originalExtension}";
        var webFileName = $"{fileName}.webp";
        var thumbnailFileName = $"{fileName}.webp";

        var originalPath = Path.Combine(originalFolder, originalFileName);
        var webPath = Path.Combine(webFolder, webFileName);
        var thumbnailPath = Path.Combine(thumbnailFolder, thumbnailFileName);

        await using (var originalStream = File.Create(originalPath))
        {
            await file.CopyToAsync(originalStream);
        }

        Console.WriteLine("STEP 10 - Original kaydedildi");

        using var webImage = image.Clone(x => { });

        Console.WriteLine("STEP 11 - Web clone oluşturuldu");

        ResizeImage(webImage, MaxImageSize);

        Console.WriteLine("STEP 12 - Web resize tamamlandı");

        await webImage.SaveAsWebpAsync(
            webPath,
            new WebpEncoder
            {
                Quality = 95
            });

        Console.WriteLine("STEP 13 - WebP kaydedildi");

        using var thumbnailImage = image.Clone(x => { });

        Console.WriteLine("STEP 14 - Thumbnail clone oluşturuldu");

        ResizeImage(thumbnailImage, ThumbnailSize);

        Console.WriteLine("STEP 15 - Thumbnail resize tamamlandı");

        await thumbnailImage.SaveAsWebpAsync(
            thumbnailPath,
            new WebpEncoder
            {
                Quality = 85
            });

        Console.WriteLine("STEP 16 - Thumbnail kaydedildi");

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