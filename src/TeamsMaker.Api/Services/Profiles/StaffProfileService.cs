using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Services.Profiles;

public class StaffProfileService
    (AppDBContext db, IUserInfo userInfo, ProfileUtilities profileUtilities) : IProfileService
{
    public async Task<GetProfileResponse> GetAsync(CancellationToken ct)
    {
        var response = new GetProfileResponse { Roles = userInfo.Roles.ToList() };

        var staff = await db.Staff
            .Include(st => st.Links)
            .SingleOrDefaultAsync(st => st.Id == userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        profileUtilities.GetStaffData(staff, response);
        profileUtilities.GetUserData(staff, response);

        response.StaffInfo!.Circles = await profileUtilities.GetStaffActiveCircles(staff.Id, ct);
        response.StaffInfo!.Archive = await profileUtilities.GetStaffArchievedCircles(staff.Id, ct);

        return response;
    }

    public async Task<GetOtherProfileResponse> GetOtherAsync(string id, CancellationToken ct)
    {
        var response = new GetOtherProfileResponse();

        var staff = await db.Staff
            .Include(st => st.Links)
            .SingleOrDefaultAsync(st => st.Id == id, ct) ??
            throw new ArgumentException("Invalid ID!");

        profileUtilities.GetOtherStaffData(staff, response);
        profileUtilities.GetOtherUserData(staff, response);

        response.StaffInfo!.Circles = await profileUtilities.GetStaffActiveCircles(id, ct);
        response.StaffInfo!.Archive = await profileUtilities.GetStaffArchievedCircles(id, ct);

        return response;
    }

    public async Task<List<GetStaffAsLookupsResponse>> GetStaffLookupsAsync(PositionEnum position, CancellationToken ct)
    {
        var lookupsQuery = db.Roles
            .Join(db.UserRoles,
                role => role.Id,
                userRole => userRole.RoleId,
                (role, userRole) => new { role, userRole }
            )
            .Join(db.Staff,
                userPermission => userPermission.userRole.UserId,
                st => st.Id,
                (userPermission, userRole) => new { userPermission, userRole })
            .AsNoTracking()
            .AsQueryable();


        if (position == PositionEnum.Head)
            lookupsQuery = lookupsQuery
                .Where(q => q.userPermission.role.Name == AppRoles.HeadOfDept);

        if (position == PositionEnum.Supervisor)
            lookupsQuery = lookupsQuery
                .Where(q => q.userPermission.role.Name == AppRoles.HeadOfDept
                        || q.userPermission.role.Name == AppRoles.Professor);

        if (position == PositionEnum.Supervisor)
            lookupsQuery = lookupsQuery
                .Where(q => q.userPermission.role.Name == AppRoles.HeadOfDept
                        || q.userPermission.role.Name == AppRoles.Professor
                        || q.userPermission.role.Name == AppRoles.Assistant);

        var result = await lookupsQuery
            .Select(staff => new GetStaffAsLookupsResponse
            {
                Id = staff.userRole.Id,
                FullName = $"{staff.userRole.FirstName} {staff.userRole.FirstName}"
            })
            .ToListAsync(cancellationToken: ct);

        throw new NotImplementedException();
    }

    public async Task UpdateAsync(UpdateProfileRequest profileRequest, CancellationToken ct)
    {
        var staff = await db.Staff
            .Include(st => st.Links)
            .SingleOrDefaultAsync(st => st.Id == userInfo.UserId, ct) ??
            throw new ArgumentException("Invalid ID!");

        profileUtilities.UpdateUserData(staff, profileRequest);

        await db.SaveChangesAsync(ct);
    }
}
