using Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;

namespace TeamsMaker.Api.Services.Storage.Interfacecs;

public interface IStorageService
{
    Task<FileContentResult?> LoadFileAsync(string folder, FileData? file, CancellationToken ct);
    Task<FileData?> UpdateFileAsync(string? oldFileName, IFormFile? newFile, string newFileName, string folder, CancellationToken ct);
}
