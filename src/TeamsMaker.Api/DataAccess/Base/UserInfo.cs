using DataAccess.Base.Interfaces;
using Microsoft.AspNetCore.Authentication;
using TeamsMaker.Api.DataAccess.Context;

namespace DataAccess.Base;

public class UserInfo : IUserInfo
{
    public string UserId { get; private set; } = string.Empty;
    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public int OrganizationId { get; private set; } = 0;
    public IEnumerable<string> Roles { get; private set; } = [];

    public UserInfo(IHttpContextAccessor accessor, AppDBContext db)
    {
        var contextClaims = accessor.HttpContext?.User?.Claims;
        if (contextClaims is null || !contextClaims.Any()) return;

        var contextUserId = contextClaims.FirstOrDefault(a => a.Type == "Id")?.Value;

        if (string.IsNullOrEmpty(contextUserId)) return;

        var user = db.Users
            .IgnoreQueryFilters()
            .AsNoTracking()
            .SingleOrDefault(row => row.Id == contextUserId);

        if (user is null) throw new ArgumentNullException("Unknown user");

        UserId = user.Id;
        UserName = user.UserName!;
        Email = user.Email!;
        OrganizationId = user.OrganizationId;
        Roles = GetRoles(db, UserId);
    }


    private IEnumerable<string> GetRoles(AppDBContext db, string userId)
    {
        var roles = db.UserRoles
            .Join(
                    db.Roles,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => new { userRole, role }
                )
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(row => row.userRole.UserId == userId)
            .Select(row => row.role.Name);

        return roles!;
    }
}
