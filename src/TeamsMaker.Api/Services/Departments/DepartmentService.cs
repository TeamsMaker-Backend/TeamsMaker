
using DataAccess.Base.Interfaces;

using TeamsMaker.Api.Contracts.Responses.Departments;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Services.Departments.Interfaces;

namespace TeamsMaker.Api.Services.Departments;

public class DepartmentService(AppDBContext db, IUserInfo userInfo) : IDepartmentService
{
    public async Task<List<GetDepartmentLookupResponse>> GetLookupsAync(CancellationToken ct)
        => await db.Departments
            .Where(d => d.OrganizationId == userInfo.OrganizationId)
            .Select(d => new GetDepartmentLookupResponse{
                Id = d.Id,
                Name = d.Name
            })
            .ToListAsync(ct);
}
