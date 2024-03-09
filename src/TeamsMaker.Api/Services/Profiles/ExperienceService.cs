using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.JsonPatch;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class ExperienceService(AppDBContext db, IUserInfo userInfo) : IExperienceService
{
    private readonly AppDBContext _db = db;
    private readonly IUserInfo _userInfo = userInfo;

    public async Task AddExperienceAsync(AddExperienceRequest experienceRequest, CancellationToken ct)
    {
        var experience = new Experience
        {
            Title = experienceRequest.Title,
            Organization = experienceRequest.Organization,
            Role = experienceRequest.Role,
            StartDate = experienceRequest.StartDate,
            EndDate = experienceRequest.EndDate,
            Description = experienceRequest.Description,
            StudentId = _userInfo.UserId,
        };

        await _db.Experiences.AddAsync(experience, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteExperienceAsync(int experienceId, CancellationToken ct)
    {
        var experience = await _db.Experiences.FindAsync([experienceId], ct) ?? throw new ArgumentException("Not found");

        _db.Experiences.Remove(experience);

        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateExperienceAsync(int experienceId, UpdateExperienceRequest updateExperienceRequest, CancellationToken ct)
    {
        var experience =
            await _db.Experiences.SingleOrDefaultAsync(ex => ex.Id == experienceId, ct) ??
            throw new ArgumentException("Invalid ID!");

        if (!string.IsNullOrEmpty(updateExperienceRequest.Title)) experience.Title = updateExperienceRequest.Title;
        if (!string.IsNullOrEmpty(updateExperienceRequest.Organization)) experience.Organization = updateExperienceRequest.Organization;
        if (!string.IsNullOrEmpty(updateExperienceRequest.Role)) experience.Role = updateExperienceRequest.Role;
        if (updateExperienceRequest.StartDate.HasValue) experience.StartDate = updateExperienceRequest.StartDate.Value;
        if (updateExperienceRequest.EndDate.HasValue) experience.EndDate = updateExperienceRequest.EndDate.Value;
        if(!string.IsNullOrEmpty(updateExperienceRequest.Description)) experience.Description = updateExperienceRequest.Description;

        await _db.SaveChangesAsync(ct);
    }
}