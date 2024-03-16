using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class StudentProfileService
    (AppDBContext db, IWebHostEnvironment host,
    IUserInfo userInfo, ProfileUtilities profileUtilities, IServiceProvider serviceProvider) : IProfileService
{
    private readonly IFileService _fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

    public async Task<List<GetStudentAsRowResponse>> FilterAsync(string query, CancellationToken ct)
    {
        var studentsQuery = db.Students.AsQueryable();

        if(!string.IsNullOrEmpty(query))
            studentsQuery.Where(std => std.FirstName.Contains(query)
                        || std.LastName.Contains(query)
                        || (std.Email != null && std.Email.Contains(query)));

        var students = await studentsQuery
            .Select(std => new GetStudentAsRowResponse{
                Id = std.Id,
                FirstName = std.FirstName,
                LastName = std.LastName,
                Bio = std.Bio,
                Avatar = _fileService.GetFileUrl(std.Id, FileTypes.Avatar)
            })
            .ToListAsync(ct);

        return students;
    }


    public async Task<GetProfileResponse> GetAsync(CancellationToken ct)
    {
        var response = new GetProfileResponse { Roles = userInfo.Roles.ToList() };

        var student =
            await db.Students
            .Include(st => st.Links)
            .Include(st => st.Experiences)
            .Include(st => st.Projects)
                .ThenInclude(p => p.Skills)
            .SingleOrDefaultAsync(st => st.Id == userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        profileUtilities.GetStudentData(student, response);
        profileUtilities.GetUserData(student, response);

        return response;
    }

    public async Task<GetOtherProfileResponse> GetOtherAsync(string id, CancellationToken ct)
    {
        var response = new GetOtherProfileResponse();

        var student = await db.Students
            .Include(st => st.Links)
            .Include(st => st.Experiences)
            .Include(st => st.Projects)
                .ThenInclude(p => p.Skills)
            .SingleOrDefaultAsync(st => st.Id == id, ct) ??
            throw new ArgumentException("Invalid ID!");

        profileUtilities.GetOtherStudentData(student, response);
        profileUtilities.GetOtherUserData(student, response);

        return response;
    }

    public async Task UpdateAsync(UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        var links = db.Links.Where(l => l.UserId == userInfo.UserId);
        db.Links.RemoveRange(links);

        var student = await db.Students
                .Include(st => st.Links)
                .SingleOrDefaultAsync(st => st.Id == userInfo.UserId, ct) ??
                throw new ArgumentException("Invalid ID!");

        profileUtilities.UpdateUserDataAsync(student, profileRequest, Path.Combine(host.WebRootPath, BaseTypes.Student), ct);

        await db.SaveChangesAsync(ct);
    }
}
