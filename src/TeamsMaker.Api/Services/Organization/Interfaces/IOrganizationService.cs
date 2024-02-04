using TeamsMaker.Api.Contracts.Requests;

namespace TeamsMaker.Api.Services.Organizations;

public interface IOrganizationService
{
    
    Task AddAsync(AddOrganizationRequest organizationRequest, CancellationToken ct);
    Task UpdateAsync(int organizationId, UpdateOrganizationRequest organizationRequest, CancellationToken ct);
    Task DeleteAsync(int organizationId, CancellationToken ct);
}
