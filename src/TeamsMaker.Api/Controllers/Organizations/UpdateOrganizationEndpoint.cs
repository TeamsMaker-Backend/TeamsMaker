using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Services.Organizations;

namespace TeamsMaker.Api.Controllers;


[Authorize]
public class UpdateOrganizationEndpoint : BaseApiController
{
    private readonly IOrganizationService _organizationService;

    public UpdateOrganizationEndpoint(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpPut("organizations/{id}")]
    public async Task<IActionResult> UpdateOrganization(int id, UpdateOrganizationRequest request, CancellationToken ct)
    {
        await _organizationService.UpdateAsync(id, request, ct);

        return NoContent();
    }
}
