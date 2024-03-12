using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class ExperienceService(AppDBContext db, IUserInfo userInfo) : IExperienceService
{
    public async Task AddAsync(AddExperienceRequest experienceRequest, CancellationToken ct)
    {
        var experience = new Experience
        {
            Title = experienceRequest.Title,
            Organization = experienceRequest.Organization,
            Role = experienceRequest.Role,
            StartDate = experienceRequest.StartDate,
            EndDate = experienceRequest.EndDate,
            Description = experienceRequest.Description,
            StudentId = userInfo.UserId,
        };

        await db.Experiences.AddAsync(experience, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int experienceId, CancellationToken ct)
    {
        var experience =
            await db.Experiences.FindAsync([experienceId], ct) ??
            throw new ArgumentException("Not found");

        db.Experiences.Remove(experience);

        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(int experienceId, UpdateExperienceRequest updateExperienceRequest, CancellationToken ct)
    {
        var experience =
            await db.Experiences.SingleOrDefaultAsync(ex => ex.Id == experienceId, ct) ??
            throw new ArgumentException("Invalid ID!");

        experience.Title = updateExperienceRequest.Title;
        experience.Organization = updateExperienceRequest.Organization;
        experience.Role = updateExperienceRequest.Role;
        experience.StartDate = updateExperienceRequest.StartDate;
        experience.EndDate = updateExperienceRequest.EndDate;
        experience.Description = updateExperienceRequest.Description;

        await db.SaveChangesAsync(ct);
    }
}