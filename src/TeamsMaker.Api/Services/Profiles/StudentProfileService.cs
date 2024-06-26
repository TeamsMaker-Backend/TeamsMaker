﻿using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Files.Interfaces;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class StudentProfileService
    (AppDBContext db, IUserInfo userInfo, ProfileUtilities profileUtilities
    , IServiceProvider serviceProvider) : IProfileService
{
    private readonly IFileService _fileService = serviceProvider.GetRequiredKeyedService<IFileService>(BaseTypes.Student);

    public async Task<GetProfileResponse> GetAsync(CancellationToken ct)
    {
        var response = new GetProfileResponse { Roles = userInfo.Roles.ToList() };

        var student =
            await db.Students
            .Include(st => st.Links)
            .Include(st => st.JoinRequests)
                .ThenInclude(jr => jr.Circle)
            .Include(st => st.Experiences)
            .Include(st => st.Projects)
                .ThenInclude(p => p.Skills)
            .Include(st => st.MemberOn)
                .ThenInclude(m => m.Circle)
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

    public Task<List<GetStaffAsLookupsResponse>> GetStaffLookupsAsync(PositionEnum position, CancellationToken ct)
    {   
        throw new NotImplementedException();
    }


    public async Task UpdateAsync(UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        var student = await db.Students
                .Include(st => st.Links)
                .SingleOrDefaultAsync(st => st.Id == userInfo.UserId, ct) ??
                throw new ArgumentException("Invalid ID!");

        profileUtilities.UpdateUserData(student, profileRequest);

        await db.SaveChangesAsync(ct);
    }
}
