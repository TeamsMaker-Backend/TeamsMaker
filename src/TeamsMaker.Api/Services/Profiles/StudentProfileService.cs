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

public class StudentProfileService(AppDBContext db, IWebHostEnvironment hostEnvironment, IUserInfo userInfo) : IProfileService, IStudentProfileService
{
    private readonly AppDBContext _db = db;
    private readonly IWebHostEnvironment _host = hostEnvironment;
    private readonly IUserInfo _user = userInfo;
    private readonly string _folder = BaseFolders.Student;

    public async Task<GetProfileResponse> GetProfileAsync(CancellationToken ct)
    {
        GetProfileResponse response = new();

        var student =
            await _db.Students.Include(x => x.Links).SingleOrDefaultAsync(x => x.Id == _user.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        ProfileUtilities.GetUserData(student, response);
        ProfileUtilities.GetStudentData(student, response);

        return response;
    }

    public async Task UpdateProfileAsync(UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        var links = _db.Links.Where(x => x.UserId == _user.UserId);
        _db.Links.RemoveRange(links);

        var student =
                await _db.Students.Include(x => x.Links).SingleOrDefaultAsync(x => x.Id == _user.UserId, ct) ??
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

    public async Task AddProjectAsync(ProjectRequest projectRequest, CancellationToken ct)
    {
       DataAccess.Models.Project project = new()
        {
            Name = projectRequest.Name,
            Url = projectRequest.Url,
            Description = projectRequest.Description,
            Tags = projectRequest.Tags,
            StudentId = _user.UserId
        };
        await _db.Projects.AddAsync(project, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateProjectAsync(int projectId, ProjectRequest projectRequest, CancellationToken ct)
    {
        var project =
            await _db.Projects.SingleOrDefaultAsync(x => x.Id == projectId, ct) ??
                throw new ArgumentException("Invalid ID!");

        project.Name = projectRequest.Name;
        project.Url = projectRequest.Url;
        project.Description = projectRequest.Description;
        project.Tags = projectRequest.Tags;

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteProjectAsync(int projectId, CancellationToken ct)
    {
        var project = await _db.Projects.FindAsync([projectId], ct);

        if (project != null)
            _db.Projects.Remove(project);

        await _db.SaveChangesAsync(ct);
    }

    public async Task AddExperienceAsync(ExperienceRequest experienceRequest, CancellationToken ct)
    {
        DataAccess.Models.Experience experience = new()
        {
            Organization = experienceRequest.Organization,
            Role = experienceRequest.Role,
            StartDate = experienceRequest.StartDate,
            EndDate = experienceRequest.EndDate,
            Description = experienceRequest.Description,
            StudentId = _user.UserId,
        };

        await _db.Experiences.AddAsync(experience, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateExperienceAsync(int experienceId, ExperienceRequest experienceRequest, CancellationToken ct)
    {
        var experience =
            await _db.Experiences.SingleOrDefaultAsync(x => x.Id == experienceId, ct) ??
                throw new ArgumentException("Invalid ID!");

        experience.Organization = experienceRequest.Organization;
        experience.Role = experienceRequest.Role;
        experience.StartDate = experienceRequest.StartDate;
        experience.EndDate = experienceRequest.EndDate;
        experience.Description = experienceRequest.Description;

        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteExperienceAsync(int experienceId, CancellationToken ct)
    {
        var experience = await _db.Experiences.FindAsync([experienceId], ct);

        if (experience != null)
            _db.Experiences.Remove(experience);

        await _db.SaveChangesAsync(ct);
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
