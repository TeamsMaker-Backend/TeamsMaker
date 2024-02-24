using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.ProfileService.Interface;
using TeamsMaker.Api.Services.ProfileService.Utilities;

namespace TeamsMaker.Api.Services.ProfileService;

public class ProfileService(AppDBContext db, IWebHostEnvironment hostEnvironment, IUserInfo userInfo) : IProfileService
{
    private readonly AppDBContext _db = db;
    private readonly IWebHostEnvironment _host = hostEnvironment;
    private readonly IUserInfo _user = userInfo;

    public async Task<ProfileResponse> GetProfileAsync(CancellationToken ct)
    {
        ProfileResponse response = new();

        if (_user.Roles.Contains(AppRoles.Student))
        {
            var student =
                await _db.Students.FindAsync(_user.UserId, ct) ??
                throw new ArgumentException("Invalid ID!");

            ProfileUtilities.GetUserData(student, response);
            ProfileUtilities.GetStudentData(student, response);
        }
        else
        {
            var staff =
                await _db.Staff.FindAsync(_user.UserId, ct) ??
                throw new ArgumentException("Invalid ID!");

            ProfileUtilities.GetUserData(staff, response);
            ProfileUtilities.GetStaffData(staff, response);
        }

        return response;
    }

    public async Task<FileContentResult?> GetAvatarAsync(Guid id, CancellationToken ct)
    {
        FileContentResult? result;
        if (await _db.Students.AnyAsync(x => x.Id == id.ToString(), ct))
        {
            var student =
                await _db.Students.FindAsync(id.ToString(), ct) ??
                throw new ArgumentException("Invalid ID!");

            result =
                await FileUtilities.GetFileAsync(Path.Combine(_host.WebRootPath, BaseFolders.Student, FileTypes.Avatar), student.Avatar, ct);
        }
        else if (await _db.Staff.AnyAsync(x => x.Id == id.ToString(), ct))
        {
            var staff =
                await _db.Staff.FindAsync(id.ToString(), ct) ??
                throw new ArgumentException("Invalid ID!");

            result =
                await FileUtilities.GetFileAsync(Path.Combine(_host.WebRootPath, BaseFolders.Staff, FileTypes.Avatar), staff.Avatar, ct);
        }
        else
            throw new ArgumentException("Invalid Data!");

        return result;
    }

    public async Task<FileContentResult?> GetHeaderAsync(Guid id, CancellationToken ct)
    {
        FileContentResult? result;
        if (await _db.Students.AnyAsync(x => x.Id == id.ToString(), ct))
        {
            var student =
                await _db.Students.FindAsync(id.ToString(), ct) ??
                throw new ArgumentException("Invalid ID!");

            result =
                await FileUtilities.GetFileAsync(Path.Combine(_host.WebRootPath, BaseFolders.Student, FileTypes.Header), student.Header, ct);
        }
        else if (await _db.Staff.AnyAsync(x => x.Id == id.ToString(), ct))
        {
            var staff =
                await _db.Staff.FindAsync(id.ToString(), ct) ??
                throw new ArgumentException("Invalid ID!");

            result =
                await FileUtilities.GetFileAsync(Path.Combine(_host.WebRootPath, BaseFolders.Staff, FileTypes.Header), staff.Header, ct);
        }
        else
            throw new ArgumentException("Invalid Data!");

        return result;
    }

    public async Task<FileContentResult?> GetCVAsync(Guid id, CancellationToken ct)
    {
        FileContentResult? result;
        if (await _db.Students.AnyAsync(x => x.Id == id.ToString(), ct))
        {
            var student =
                await _db.Students.FindAsync(id.ToString(), ct) ??
                throw new ArgumentException("Invalid ID!");

            result =
                await FileUtilities.GetFileAsync(Path.Combine(_host.WebRootPath, BaseFolders.Student, FileTypes.CV), student.CV, ct);
        }
        else
            throw new ArgumentException("Invalid Data!");

        return result;
    }

    public async Task UpdateProfileAsync(UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        if (_user.Roles.Contains(AppRoles.Student))
        {
            var student =
                await _db.Students.FindAsync(_user.UserId, ct) ??
                throw new ArgumentException("Invalid ID!");

            ProfileUtilities.UpdateUserDataAsync(student, profileRequest, Path.Combine(_host.WebRootPath, BaseFolders.Student), ct);
        }
        else
        {
            var staff =
                await _db.Staff.FindAsync(_user.UserId, ct) ??
                throw new ArgumentException("Invalid ID!");

            ProfileUtilities.UpdateUserDataAsync(staff, profileRequest, Path.Combine(_host.WebRootPath, BaseFolders.Staff), ct);
        }

        await _db.SaveChangesAsync(ct);
    }
}
