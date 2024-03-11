using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.File;

namespace TeamsMaker.Api.Services.Files.Interfaces;

public interface IFileService
{
    Task UpdateFileAsync(string id, string fileType, UpdateFileRequest request, CancellationToken ct);
    Task<FileContentResult?> GetFileContentAsync(string id, string fileType, CancellationToken ct);
    string? GetFileUrl(string id, string fileType); //TODO: [Refactor] Remove dublicates Urgent
}
