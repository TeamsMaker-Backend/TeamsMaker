﻿using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class StaffProfileService
    (AppDBContext db, IWebHostEnvironment host, IUserInfo userInfo, ProfileUtilities profileUtilities) : IProfileService
{
    public async Task<GetProfileResponse> GetAsync(CancellationToken ct)
    {
        var response = new GetProfileResponse { Roles = userInfo.Roles.ToList() };

        var staff =
            await db.Staff
            .Include(st => st.Links)
            .SingleOrDefaultAsync(st => st.Id == userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        profileUtilities.GetStaffData(staff, response);
        profileUtilities.GetUserData(staff, response);

        return response;
    }

    public async Task<GetOtherProfileResponse> GetOtherAsync(string id, CancellationToken ct)
    {
        var response = new GetOtherProfileResponse();

        var staff =
            await db.Staff
            .Include(st => st.Links)
            .SingleOrDefaultAsync(st => st.Id == id, ct) ??
            throw new ArgumentException("Invalid ID!");

        profileUtilities.GetOtherStaffData(staff, response);
        profileUtilities.GetOtherUserData(staff, response);

        return response;
    }

    public async Task UpdateAsync(UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        var staff =
            await db.Staff
            .Include(st => st.Links)
            .SingleOrDefaultAsync(st => st.Id == userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        profileUtilities.UpdateUserDataAsync(staff, profileRequest, Path.Combine(host.WebRootPath, BaseTypes.Staff), ct);

        await db.SaveChangesAsync(ct);
    }
}
