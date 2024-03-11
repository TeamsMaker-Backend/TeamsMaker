using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests;

using TeamsMaker.Api.Services.Organizations;

namespace TeamsMaker.Api.Controllers.Organizations;


[Authorize]
public class AddOrganizationEndpoint(IOrganizationService organizationService) : BaseApiController
{
    [Tags("organizations")]
    [HttpPost("organizations")]
    public async Task<IActionResult> AddOrganization([FromForm] OrganizationRequest request, CancellationToken ct)
    {
        try
        {
            await organizationService.AddAsync(request, ct);
        }
        catch (Exception ex)
        {
            return BadRequest(_response.FailureResponse(ex.Message));
        }

        return Created();
    }
}
