using Microsoft.AspNetCore.Mvc;

namespace TeamsMaker.Api.Services.Files.Interfaces;

public interface IFileService
{
    Task<FileContentResult?> GetFileContentAsync(Guid id, string fileType, CancellationToken ct);
}
