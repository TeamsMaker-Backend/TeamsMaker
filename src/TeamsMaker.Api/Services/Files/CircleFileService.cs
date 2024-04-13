using Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.File;
using TeamsMaker.Api.Controllers.Files;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Storage.Interfacecs;

namespace TeamsMaker.Api.Services.Files;

public class CircleFileService
    (AppDBContext db, IStorageService storageService, IWebHostEnvironment host, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : IFileService
{
    private const string BaseType = BaseTypes.Circle;

    public async Task UpdateFileAsync(string id, string fileType, UpdateFileRequest request, CancellationToken ct)
    {
        var circle = await db.Circles.FindAsync([Guid.Parse(id)], ct) ??
            throw new ArgumentException("Invalid Circle ID");

        var result = await storageService.UpdateFileAsync(
                oldFileName: GetData(circle, fileType)?.Name,
                newFile: request.File,
                newFileName: CreateName(fileType, request.File?.FileName),
                folder: Path.Combine(host.WebRootPath, BaseType, id), ct);

        if (fileType == FileTypes.Avatar)
            circle.Avatar = result;
        else if (fileType == FileTypes.Header)
            circle.Header = result;
        else
            throw new ArgumentException("Invalid File Type");

        await db.SaveChangesAsync(ct);
    }

    public async Task<FileContentResult?> GetFileContentAsync(string id, string fileType, CancellationToken ct)
    {
        var circle =
            await db.Circles.FindAsync([Guid.Parse(id)], ct) ??
            throw new ArgumentException("Invalid ID!");

        var result =
            await storageService.LoadFileAsync(Path.Combine(host.WebRootPath, BaseType, id), GetData(circle, fileType), ct);

        return result;
    }

    public string? GetFileUrl(string id, string fileType)
    {
        if (httpContextAccessor.HttpContext is null)
            throw new ArgumentException("Http Context is Null!");

        var url = linkGenerator.GetPathByAction(
            httpContext: httpContextAccessor.HttpContext,
            action: nameof(GetFileEndpoint.File),
            controller: nameof(GetFileEndpoint),
            values: new { baseType = BaseType, id = id.ToUpper(), fileType });

        return url;
    }

    private static FileData? GetData(Circle circle, string file)
        => file switch
        {
            FileTypes.Avatar => circle.Avatar,
            FileTypes.Header => circle.Header,
            _ => null,
        };

    private static string CreateName(string fileType, string? file)
        => $"{fileType}{Path.GetExtension(file)}";
}
