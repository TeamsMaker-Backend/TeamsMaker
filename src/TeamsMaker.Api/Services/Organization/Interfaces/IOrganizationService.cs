using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Contracts.Responses;

namespace TeamsMaker.Api.Services.Organizations;

public interface IOrganizationService
{
    Task<List<GetOrganizationResponse>> GetAsync(CancellationToken ct);
    Task AddAsync(AddOrganizationRequest organizationRequest, CancellationToken ct);
    Task UpdateAsync(int organizationId, UpdateOrganizationRequest organizationRequest, CancellationToken ct);
    Task DeleteAsync(int organizationId, CancellationToken ct);
}
