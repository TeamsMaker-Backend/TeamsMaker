using Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Storage.Interfacecs;

namespace TeamsMaker.Api.Services.Storage;

public class StorageService : IStorageService
{
    public async Task<FileContentResult?> LoadFileAsync(string folder, FileData? file, CancellationToken ct)
    {
        if (file == null || File.Exists(Path.Combine(folder, file.Name)) == false)
            return null;

        var path = Path.Combine(folder, file.Name);

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

        FileContentResult contentResult = new(await File.ReadAllBytesAsync(path, ct), file.ContentType);

        return contentResult;
    }

    public async Task<FileData?> UpdateFileAsync(string? oldFile, IFormFile? newFile, string newFileName, string folder, CancellationToken ct)
    {
        if (oldFile != null && File.Exists(Path.Combine(folder, oldFile)))
            File.Delete(Path.Combine(folder, oldFile));

        if (newFile == null || newFile.Length == 0)
            return null;

        Directory.CreateDirectory(folder);

        var newFilePath = Path.Combine(folder, newFileName.ToLower());

        using (var stream = new FileStream(newFilePath, FileMode.Create))
        {
            await newFile.CopyToAsync(stream, ct);
        }

        return new FileData { Name = newFileName.ToLower(), ContentType = newFile.ContentType };
    }
}
