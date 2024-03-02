using Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Storage.Interfacecs;

namespace TeamsMaker.Api.Services.Files;

public class StudentFileService(AppDBContext db, IStorageService storageService, IWebHostEnvironment host) : IFileService
{
    private readonly AppDBContext _db = db;
    private readonly IStorageService _storageService = storageService;
    private readonly IWebHostEnvironment _host = host;
    private const string Folder = BaseTypes.Student;

    public async Task<FileContentResult?> GetFileContentAsync(Guid id, string fileType, CancellationToken ct)
    {
        var student =
            await _db.Students.FindAsync([id.ToString()], ct) ??
            throw new ArgumentException("Invalid ID!");

        var result =
            await _storageService.LoadFileAsync(Path.Combine(_host.WebRootPath, Folder, id.ToString()), GetData(student, fileType), ct);

        return result;
    }

    private static FileData? GetData(Student student, string file)
        => file switch
        {
            FileTypes.Avatar => student.Avatar,
            FileTypes.Header => student.Header,
            FileTypes.CV => student.CV,
            _ => null,
        };
}
