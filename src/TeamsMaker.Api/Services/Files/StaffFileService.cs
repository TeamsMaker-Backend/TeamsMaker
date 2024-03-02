using Core.ValueObjects;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Storage.Interfacecs;

namespace TeamsMaker.Api.Services.Files;

public class StaffFileService(AppDBContext db, IStorageService storageService, IWebHostEnvironment host) : IFileService
{
    private readonly AppDBContext _db = db;
    private readonly IStorageService _storageService = storageService;
    private readonly IWebHostEnvironment _host = host;
    private const string Folder = BaseTypes.Staff;

    public async Task<FileContentResult?> GetFileContentAsync(Guid id, string fileType, CancellationToken ct)
    {
        var staff =
            await _db.Staff.FindAsync([id.ToString()], ct) ??
            throw new ArgumentException("Invalid ID!");

        var result =
            await _storageService.LoadFileAsync(Path.Combine(_host.WebRootPath, Folder, id.ToString()), GetData(staff, fileType), ct);

        return result;
    }

    private static FileData? GetData(Staff staff, string file)
        => file switch
        {
            FileTypes.Avatar => staff.Avatar,
            FileTypes.Header => staff.Header,
            _ => null,
        };
}
