using TeamsMaker.Api.Contracts.Responses.Departments;

namespace TeamsMaker.Api.Services.Departments.Interfaces;

public interface IDepartmentService
{
    Task<List<GetDepartmentLookupResponse>> GetLookupsAync(CancellationToken ct);
}