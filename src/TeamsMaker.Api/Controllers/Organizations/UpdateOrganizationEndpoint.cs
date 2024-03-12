using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Services.Organizations;


namespace TeamsMaker.Api.Controllers.Organizations;

[Authorize]
public class UpdateOrganizationEndpoint(IOrganizationService organizationService) : BaseApiController
{
    [Tags("organizations")]
    [HttpPut("organizations/{id}")]
    public async Task<IActionResult> UpdateOrganization(int id, [FromForm] OrganizationRequest request, CancellationToken ct)
    {
        try
        {
            await organizationService.UpdateAsync(id, request, ct);
        }
        catch (Exception ex)
        {
            return BadRequest(_response.FailureResponse(ex.Message));
        }

        return NoContent();
    }
}
