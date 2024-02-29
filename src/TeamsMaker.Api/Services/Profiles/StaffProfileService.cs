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

public class StaffProfileService(AppDBContext db, IWebHostEnvironment hostEnvironment, IUserInfo userInfo) : IProfileService
{
    private readonly AppDBContext _db = db;
    private readonly IWebHostEnvironment _host = hostEnvironment;
    private readonly IUserInfo _userInfo = userInfo;
    private readonly string _folder = BaseFolders.Staff;

    public async Task<GetProfileResponse> GetProfileAsync(CancellationToken ct)
    {
        var response = new GetProfileResponse { Roles = _userInfo.Roles.ToList() };

        var staff =
            await _db.Staff.Include(x => x.Links).SingleOrDefaultAsync(x => x.Id == _userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        ProfileUtilities.GetUserData(staff, response);
        ProfileUtilities.GetStaffData(staff, response);

        return response;
    }

    public async Task UpdateProfileAsync(UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        var links = _db.Links.Where(x => x.UserId == _userInfo.UserId);
        _db.Links.RemoveRange(links);

        var staff =
                await _db.Staff
                .Include(x => x.Links)
                .SingleOrDefaultAsync(x => x.Id == _userInfo.UserId, ct) ??
                throw new ArgumentException("Invalid ID!");

        ProfileUtilities.UpdateUserDataAsync(staff, profileRequest, Path.Combine(_host.WebRootPath, BaseFolders.Staff), ct);

        await _db.SaveChangesAsync(ct);
    }

    public async Task<FileContentResult?> GetFileContentAsync(Guid id, string fileType, CancellationToken ct)
    {
        var staff =
            await _db.Staff.FindAsync([id.ToString()], ct) ??
            throw new ArgumentException("Invalid ID!");

        var result =
            await FileUtilities.LoadFileAsync(Path.Combine(_host.WebRootPath, _folder, fileType), GetData(staff, fileType), ct);

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
