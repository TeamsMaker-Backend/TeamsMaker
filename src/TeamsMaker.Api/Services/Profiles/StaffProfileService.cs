using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class StaffProfileService
    (AppDBContext db, IWebHostEnvironment hostEnvironment, IUserInfo userInfo, ProfileUtilities profileUtilities) : IProfileService
{
    private readonly AppDBContext _db = db;
    private readonly IWebHostEnvironment _host = hostEnvironment;
    private readonly IUserInfo _userInfo = userInfo;
    private readonly ProfileUtilities _profileUtilities = profileUtilities;

    public async Task<GetProfileResponse> GetProfileAsync(CancellationToken ct)
    {
        var response = new GetProfileResponse { Roles = _userInfo.Roles.ToList() };

        var staff =
            await _db.Staff
            .Include(st => st.Links)
            .SingleOrDefaultAsync(st => st.Id == _userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        _profileUtilities.GetUserData(staff, response);
        _profileUtilities.GetStaffData(staff, response);

        return response;
    }

    public async Task<GetOtherProfileResponse> GetOtherProfileAsync(string id, CancellationToken ct)
    {
        var response = new GetOtherProfileResponse();

        var staff =
            await _db.Staff
            .Include(st => st.Links)
            .SingleOrDefaultAsync(st => st.Id == id, ct) ??
            throw new ArgumentException("Invalid ID!");

        _profileUtilities.GetOtherUserData(staff, response);
        _profileUtilities.GetOtherStaffData(staff, response);

        return response;
    }

    public async Task UpdateProfileAsync(UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        var staff =
            await _db.Staff
            .Include(st => st.Links)
            .SingleOrDefaultAsync(st => st.Id == _userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        _profileUtilities.UpdateUserDataAsync(staff, profileRequest, Path.Combine(_host.WebRootPath, BaseTypes.Staff), ct);

        await _db.SaveChangesAsync(ct);
    }
}
