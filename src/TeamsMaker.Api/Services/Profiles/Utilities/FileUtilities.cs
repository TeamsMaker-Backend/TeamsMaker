using Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;

namespace TeamsMaker.Api.Services.Profiles.Utilities;

public static class FileUtilities
{
    public static async Task<FileContentResult?> LoadFileAsync(string folder, FileData? file, CancellationToken ct)
    {
        if (file == null)
            return null;

        var path = Path.Combine(folder, file.Name);

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        FileContentResult contentResult = new(await File.ReadAllBytesAsync(path, ct), file.ContentType);

        return contentResult;
    }

    public static async Task<FileData?> UpdateFileAsync(string? oldFile, IFormFile? newFile, string newFileName, string folder, CancellationToken ct)
    {
        if (oldFile != null && File.Exists(Path.Combine(folder, oldFile)))
            File.Delete(Path.Combine(folder, oldFile));

        if (newFile == null || newFile.Length == 0)
            return null;

        Directory.CreateDirectory(folder);

        var newFilePath = Path.Combine(folder, newFileName!);

        FileData file = new(newFileName, newFile.ContentType);

        using (var stream = new FileStream(newFilePath, FileMode.Create))
        {
            await newFile.CopyToAsync(stream, ct);
        }

        return file;
    }

    public static string CreateName(string id, string? file)
        => $"{id}{Path.GetExtension(file)}";
}
