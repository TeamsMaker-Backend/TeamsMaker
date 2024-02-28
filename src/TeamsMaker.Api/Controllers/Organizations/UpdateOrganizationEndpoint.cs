using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Services.Organizations;


namespace TeamsMaker.Api.Controllers.Organizations;

[Authorize]
public class UpdateOrganizationEndpoint : BaseApiController
{
    private readonly IOrganizationService _organizationService;

    public UpdateOrganizationEndpoint(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [Tags("Organizations")]
    [HttpPut("organizations/{id}")]
    public async Task<IActionResult> UpdateOrganization(int id, UpdateOrganizationRequest request, CancellationToken ct)
    {
        try
        {
            await _organizationService.UpdateAsync(id, request, ct);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(_response.FailureResponse(ex.Message));
        }
    }
}
