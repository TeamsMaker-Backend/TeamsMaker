﻿using Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.File;
using TeamsMaker.Api.Controllers.Files;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Storage.Interfacecs;

namespace TeamsMaker.Api.Services.Files;

public class StaffFileService
    (AppDBContext db, IStorageService storageService, IWebHostEnvironment host, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : IFileService
{
    private const string BaseType = BaseTypes.Staff;

    public async Task UpdateFileAsync(string id, string fileType, UpdateFileRequest request, CancellationToken ct)
    {
        var staff = await db.Staff.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid Staff ID");

        if (fileType == FileTypes.Avatar)
        {
            staff.Avatar = await storageService.UpdateFileAsync(
                oldFileName: staff.Avatar?.Name,
                newFile: request.File,
                newFileName: CreateName(FileTypes.Avatar, request.File?.FileName),
                folder: Path.Combine(host.WebRootPath, BaseType, id), ct);
        }
        else if (fileType == FileTypes.Header)
        {
            staff.Header = await storageService.UpdateFileAsync(
                oldFileName: staff.Header?.Name,
                newFile: request.File,
                newFileName: CreateName(FileTypes.Header, request.File?.FileName),
                folder: Path.Combine(host.WebRootPath, BaseType, id), ct);
        }
        else
            throw new ArgumentException("Invalid File Type");

        await db.SaveChangesAsync(ct);
    }

    public async Task<FileContentResult?> GetFileContentAsync(string id, string fileType, CancellationToken ct)
    {
        var staff =
            await db.Staff.FindAsync([id.ToString()], ct) ??
            throw new ArgumentException("Invalid ID!");

        var result =
            await storageService.LoadFileAsync(Path.Combine(host.WebRootPath, BaseType, id), GetData(staff, fileType), ct);

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

    private static FileData? GetData(Staff staff, string file)
        => file switch
        {
            FileTypes.Avatar => staff.Avatar,
            FileTypes.Header => staff.Header,
            _ => null,
        };

    private static string CreateName(string fileType, string? file)
        => $"{fileType}{Path.GetExtension(file)}";
}
