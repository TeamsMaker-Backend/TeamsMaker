using Microsoft.AspNetCore.Mvc;

namespace TeamsMaker.Api.Services.Files.Interfaces;

public interface IFileService
{
    Task<FileContentResult?> GetFileContentAsync(string id, string fileType, CancellationToken ct);
    string? GetFileUrl(string id, string fileType); // [Refactor] Remove dublicates Urgent
}
