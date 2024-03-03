using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Organizations;


namespace TeamsMaker.Api.Controllers.Organizations;

[Authorize]
public class DeleteOrganizationEndpoint(IOrganizationService organizationService) : BaseApiController
{
    private readonly IOrganizationService _organizationService = organizationService;

    [Tags("organizations")]
    [HttpDelete("organizations/{id}")]
    public async Task<IActionResult> UpdateOrganization(int id, CancellationToken ct)
    {
        try
        {
            await _organizationService.DeleteAsync(id, ct);
        }
        catch (Exception ex)
        {
            return BadRequest(_response.FailureResponse(ex.Message));
        }

        return NoContent();
    }
}
