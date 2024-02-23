using Microsoft.AspNetCore.Mvc;

namespace TeamsMaker.Api.Services.ProfileService.Utilities;

public static class FileUtilities
{
    public static async Task<FileContentResult?> GetFileAsync(string folder, string? file, CancellationToken ct)
    {
        if (file == null)
            return null;

        var path = Path.Combine(folder, file);

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        FileContentResult contentResult = new(await File.ReadAllBytesAsync(path, ct), GetContentType(path));

        return contentResult;
    }

    public static async Task<string?> UpdateFileAsync(string? oldFile, IFormFile? newFile, string? newFileName, string folder, CancellationToken ct)
    {
        if (oldFile != null && File.Exists(Path.Combine(folder, oldFile)))
            File.Delete(Path.Combine(folder, oldFile));

        if (newFile == null || newFile.Length == 0)
            return null;

        var newFilePath = Path.Combine(folder, newFileName!);

        using (var stream = new FileStream(newFilePath, FileMode.Create))
        {
            await newFile.CopyToAsync(stream, ct);
        }

        return newFileName;
    }

    private static string GetContentType(string file)
    {
        var extension = Path.GetExtension(file);
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            ".doc" => "text/msword",
            ".md" => "application/markdown",
            ".csv" => "text/csv",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".svg" => "image/svg+xml",
            ".webp" => "image/webp",
            _ => "application/octet-stream",
        };
    }

    public static string? CreateName(string id, string? file)
        => $"{id}{Path.GetExtension(file) ?? "NA"}";
}
