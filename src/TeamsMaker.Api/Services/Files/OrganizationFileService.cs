using Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.File;
using TeamsMaker.Api.Controllers.Files;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Storage.Interfacecs;

namespace TeamsMaker.Api.Services.Files;

public class OrganizationFileService
    (AppDBContext db, IStorageService storageService, IWebHostEnvironment host, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : IFileService
{
    private const string BaseType = BaseTypes.Organization;

    public async Task UpdateFileAsync(string id, string fileType, UpdateFileRequest request, CancellationToken ct)
    {
        var organization = await db.Organizations.FindAsync([int.Parse(id)], ct) ??
            throw new ArgumentException("Invalid Organization ID");

        var result = await storageService.UpdateFileAsync(
                oldFileName: GetData(organization, fileType)?.Name,
                newFile: request.File,
                newFileName: CreateName(fileType, request.File?.FileName),
                folder: Path.Combine(host.WebRootPath, BaseType, id), ct);

        if (fileType == FileTypes.Logo)
            organization.Logo = result;
        else
            throw new ArgumentException("Invalid File Type");

        await db.SaveChangesAsync(ct);
    }

    public async Task<FileContentResult?> GetFileContentAsync(string id, string fileType, CancellationToken ct)
    {
        var organization =
            await db.Organizations.FindAsync([int.Parse(id)], ct) ??
            throw new ArgumentException("Invalid ID!");

        var result =
            await storageService.LoadFileAsync(Path.Combine(host.WebRootPath, BaseType, id), GetData(organization, fileType), ct);

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
            values: new { baseType = BaseType, id, fileType });

        return url;
    }

    private static FileData? GetData(Organization organization, string file)
        => file switch
        {
            FileTypes.Logo => organization.Logo,
            _ => null,
        };

    private static string CreateName(string fileType, string? file)
        => $"{fileType}{Path.GetExtension(file)}";
}
