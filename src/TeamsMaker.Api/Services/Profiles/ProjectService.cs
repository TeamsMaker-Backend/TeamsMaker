using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class ProjectService(AppDBContext db, IUserInfo userInfo) : IProjectService
{
    public async Task AddAsync(AddProjectRequest projectRequest, CancellationToken ct)
    {
        var project = new Project
        {
            StudentId = userInfo.UserId,
            Name = projectRequest.Name,
            Url = projectRequest.Url,
            Description = projectRequest.Description,
            StartDate = projectRequest.StartDate,
            EndDate = projectRequest.EndDate,
            Skills = projectRequest.Skills?.Select(s => new Skill { Name = s }).ToList() ?? []
        };

        await db.Projects.AddAsync(project, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int projectId, CancellationToken ct)
    {
        await DeleteSkillsAsync(projectId, ct);

        var project =
            await db.Projects.FindAsync([projectId], ct) ??
            throw new ArgumentException("Not found"); ;

        db.Projects.Remove(project);

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(int projectId, AddProjectRequest projectRequest, CancellationToken ct)
    {
        await DeleteSkillsAsync(projectId, ct);

        var project =
            await db.Projects
            .Include(prj => prj.Skills)
            .SingleOrDefaultAsync(prj => prj.Id == projectId, ct) ??
            throw new ArgumentException("Invalid ID!");

        project.Name = projectRequest.Name;
        project.Url = projectRequest.Url;
        project.Description = projectRequest.Description;
        project.StartDate = projectRequest.StartDate;
        project.EndDate = projectRequest.EndDate;
        project.Skills = projectRequest.Skills?.Select(s => new Skill { Name = s }).ToList() ?? [];

        await db.SaveChangesAsync(ct);
    }

    private async Task DeleteSkillsAsync(int projectId, CancellationToken ct)
    {
        var skills = db.Skills.Where(s => s.ProjectId == projectId);
        db.Skills.RemoveRange(skills);

        await db.SaveChangesAsync(ct);
    }
}
