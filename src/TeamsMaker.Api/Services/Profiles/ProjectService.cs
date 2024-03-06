using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class ProjectService(AppDBContext db, IUserInfo userInfo) : IProjectService
{
    private readonly AppDBContext _db = db;
    private readonly IUserInfo _userInfo = userInfo;


    public async Task AddProjectAsync(AddProjectRequest projectRequest, CancellationToken ct)
    {
        var project = new Project
        {
            StudentId = _userInfo.UserId,
            Name = projectRequest.Name,
            Url = projectRequest.Url,
            Description = projectRequest.Description,
            Skills = projectRequest.Skills?.Select(s => new Skill { Name = s }).ToList() ?? []
        };

        await _db.Projects.AddAsync(project, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteProjectAsync(int projectId, CancellationToken ct)
    {
        await DeleteSkillsAsync(projectId, ct);

        var project = await _db.Projects.FindAsync([projectId], ct) ?? throw new ArgumentException("Not found"); ;

        _db.Projects.Remove(project);

        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateProjectAsync(int projectId, AddProjectRequest projectRequest, CancellationToken ct)
    {
        await DeleteSkillsAsync(projectId, ct);

        var project =
            await _db.Projects
            .Include(prj => prj.Skills)
            .SingleOrDefaultAsync(prj => prj.Id == projectId, ct) ??
            throw new ArgumentException("Invalid ID!");

        project.Name = projectRequest.Name;
        project.Url = projectRequest.Url;
        project.Description = projectRequest.Description;
        project.Skills = projectRequest.Skills?.Select(s => new Skill { Name = s }).ToList() ?? [];

        await _db.SaveChangesAsync(ct);
    }

    private async Task DeleteSkillsAsync(int projectId, CancellationToken ct)
    {
        var skills = _db.Skills.Where(s => s.ProjectId == projectId);
        _db.Skills.RemoveRange(skills);

        await _db.SaveChangesAsync(ct);
    }
}
