using Core.ValueObjects;

using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Api.Services.Profiles.Utilities;

namespace TeamsMaker.Api.Services.Profiles;

public class StudentProfileService(AppDBContext db, IWebHostEnvironment hostEnvironment, IUserInfo userInfo) : IProfileService
{
    private readonly AppDBContext _db = db;
    private readonly IWebHostEnvironment _host = hostEnvironment;
    private readonly IUserInfo _userInfo = userInfo;
    private readonly string _folder = BaseFolders.Student;

    public async Task<GetProfileResponse> GetProfileAsync(CancellationToken ct)
    {
        var response = new GetProfileResponse { Roles = _userInfo.Roles.ToList() };

        var student =
            await _db.Students
            .Include(st => st.Links)
            .Include(st => st.Experiences)
            .Include(st => st.Projects).ThenInclude(p => p.Skills)
            .SingleOrDefaultAsync(st => st.Id == _userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        ProfileUtilities.GetUserData(student, response);
        ProfileUtilities.GetStudentData(student, response);

        return response;
    }

    public async Task<GetOtherProfileResponse> GetOtherProfileAsync(string id, CancellationToken ct)
    {
        var response = new GetOtherProfileResponse();

        var student = await _db.Students
            .Include(st => st.Links)
            .Include(st => st.Experiences)
            .Include(st => st.Projects)
                .ThenInclude(p => p.Skills)
            .SingleOrDefaultAsync(st => st.Id == id, ct) ??
            throw new ArgumentException("Invalid ID!");

        ProfileUtilities.GetOtherUserData(student, response);
        ProfileUtilities.GetOtherStudentData(student, response);

        return response;
    }

    public async Task UpdateProfileAsync(UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        var links = _db.Links.Where(l => l.UserId == _userInfo.UserId);
        _db.Links.RemoveRange(links);

        var student = await _db.Students
                .Include(st => st.Links)
                .SingleOrDefaultAsync(st => st.Id == _userInfo.UserId, ct) ??
                throw new ArgumentException("Invalid ID!");

        ProfileUtilities.UpdateUserDataAsync(student, profileRequest, Path.Combine(_host.WebRootPath, BaseFolders.Student), ct);

        await _db.SaveChangesAsync(ct);
    }

    public async Task<FileContentResult?> GetFileContentAsync(Guid id, string fileType, CancellationToken ct)
    {
        var student =
            await _db.Students.FindAsync([id.ToString()], ct) ??
            throw new ArgumentException("Invalid ID!");

        var result =
            await FileUtilities.LoadFileAsync(Path.Combine(_host.WebRootPath, _folder, fileType), GetData(student, fileType), ct);

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
