using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class StudentProfileService
    (AppDBContext db, IWebHostEnvironment host, IUserInfo userInfo, ProfileUtilities profileUtilities) : IProfileService
{
    private readonly AppDBContext _db = db;
    private readonly IWebHostEnvironment _host = host;
    private readonly IUserInfo _userInfo = userInfo;
    private readonly ProfileUtilities _profileUtilities = profileUtilities;

    public async Task<GetProfileResponse> GetProfileAsync(CancellationToken ct)
    {
        var response = new GetProfileResponse { Roles = _userInfo.Roles.ToList() };

        var student =
            await _db.Students
            .Include(st => st.Links)
            .Include(st => st.Experiences)
            .Include(st => st.Projects)
                .ThenInclude(p => p.Skills)
            .SingleOrDefaultAsync(st => st.Id == _userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        _profileUtilities.GetStudentData(student, response);
        _profileUtilities.GetUserData(student, response);

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

        _profileUtilities.GetOtherStudentData(student, response);
        _profileUtilities.GetOtherUserData(student, response);

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

        _profileUtilities.UpdateUserDataAsync(student, profileRequest, Path.Combine(_host.WebRootPath, BaseTypes.Student), ct);

        await _db.SaveChangesAsync(ct);
    }
}
