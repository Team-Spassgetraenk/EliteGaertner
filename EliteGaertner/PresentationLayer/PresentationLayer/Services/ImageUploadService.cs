using Microsoft.AspNetCore.Components.Forms;

namespace PresentationLayer.Services;

public interface IImageUploadService
{
    //Speichert das Bild ab und gibt ein URL Link zurück
    Task<string> SaveImageAsync(IBrowserFile file, string folder = "pictures/uploads", long maxBytes = 5 * 1024 * 1024);
    
    //Löscht die Datei anhand der relativen url ("uploads"/...)
    bool TryDeleteByRelativeUrl(string? relativeUrl);
}

public sealed class ImageUploadService : IImageUploadService
{
    //Prüft welche Dateiendungen benutzt werden dürfen
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };

    //Gibt dem Programm Infos über das SystemInterfacen
    //z.B. wo liegt wwwroot?
    private readonly IWebHostEnvironment _env;

    public ImageUploadService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveImageAsync(IBrowserFile file, string folder = "pictures/uploads", long maxBytes = 5 * 1024 * 1024)
    {
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        //Extension prüfen (einfacher Schutz)
        var ext = Path.GetExtension(file.Name);
        if (string.IsNullOrWhiteSpace(ext) || !AllowedExtensions.Contains(ext))
            throw new InvalidOperationException($"Dateityp '{ext}' ist nicht erlaubt. Erlaubt: {string.Join(", ", AllowedExtensions)}");

        if (file.Size <= 0)
            throw new InvalidOperationException("Datei ist leer.");

        if (file.Size > maxBytes)
            throw new InvalidOperationException($"Datei ist zu groß. Max: {maxBytes} Bytes");

        //Zielordner sicherstellen (unter wwwroot)
        //Standard: wwwroot/pictures/uploads
        var safeFolder = string.IsNullOrWhiteSpace(folder)
            ? "pictures/uploads"
            : folder.Trim().Trim('/','\\');
        var targetDir = Path.Combine(_env.WebRootPath, safeFolder);
        Directory.CreateDirectory(targetDir);

        // UUID-Dateiname generieren + Original-Extension beibehalten
        var extension = Path.GetExtension(file.Name);
        var fileName = $"{Guid.NewGuid().ToString("N")}{extension}";
        var absolutePath = Path.Combine(targetDir, fileName);

        //Stream speichern
        await using (var stream = file.OpenReadStream(maxBytes))
        await using (var fs = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
        {
            await stream.CopyToAsync(fs);
        }

        // Relative URL zurückgeben (mit führendem Slash, damit es direkt als img src funktioniert)
        return $"/{safeFolder}/{fileName}";
    }

    public bool TryDeleteByRelativeUrl(string? relativeUrl)
    {
        if (string.IsNullOrWhiteSpace(relativeUrl))
            return false;

        var cleaned = relativeUrl.Trim().TrimStart('/');

        // Sicherheitscheck: kein Directory Traversal
        if (cleaned.Contains(".."))
            return false;

        var absolute = Path.Combine(_env.WebRootPath, cleaned.Replace('/', Path.DirectorySeparatorChar));
        if (!File.Exists(absolute))
            return false;

        File.Delete(absolute);
        return true;
    }
}