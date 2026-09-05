namespace LookTwiice.Services.Implementations
{
    public class FileService : Services.Interfaces.IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Dosya seçilmedi.", nameof(file));

            var uploadsFolder = Path.Combine(_environment.WebRootPath, folder);
            Directory.CreateDirectory(uploadsFolder);

            var originalFileName = Path.GetFileName(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}_{originalFileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{folder}/{uniqueFileName}";
        }

        public void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(
                _environment.WebRootPath,
                filePath.TrimStart('/')
            );

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}