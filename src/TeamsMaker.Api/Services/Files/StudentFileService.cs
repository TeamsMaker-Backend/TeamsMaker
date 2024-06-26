﻿using Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.File;
using TeamsMaker.Api.Controllers.Files;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Storage.Interfacecs;

namespace TeamsMaker.Api.Services.Files;

public class StudentFileService
    (AppDBContext db, IStorageService storageService, IWebHostEnvironment host, IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : IFileService
{
    private const string BaseType = BaseTypes.Student;

    public async Task UpdateFileAsync(string id, string fileType, UpdateFileRequest request, CancellationToken ct)
    {
        var student = await db.Students.FindAsync([id], ct) ??
            throw new ArgumentException("Invalid student ID");

        var result = await storageService.UpdateFileAsync(
                oldFileName: GetData(student, fileType)?.Name,
                newFile: request.File,
                newFileName: CreateName(fileType, request.File?.FileName),
                folder: Path.Combine(host.WebRootPath, BaseType, id), ct);

        if (fileType == FileTypes.Avatar)
            student.Avatar = result;
        else if (fileType == FileTypes.Header)
            student.Header = result;
        else if (fileType == FileTypes.CV)
            student.CV = result;
        else
            throw new ArgumentException("Invalid File Type");

        await db.SaveChangesAsync(ct);
    }

    public async Task<FileContentResult?> GetFileContentAsync(string id, string fileType, CancellationToken ct)
    {
        var student =
            await db.Students.FindAsync([id.ToString()], ct) ??
            throw new ArgumentException("Invalid ID!");

        var result =
            await storageService.LoadFileAsync(Path.Combine(host.WebRootPath, BaseType, id), GetData(student, fileType), ct);

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

    private static FileData? GetData(Student student, string file)
        => file switch
        {
            FileTypes.Avatar => student.Avatar,
            FileTypes.Header => student.Header,
            FileTypes.CV => student.CV,
            _ => null,
        };

    private static string CreateName(string fileType, string? file)
        => $"{fileType}{Path.GetExtension(file)}";
}
