using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Organizations;


namespace TeamsMaker.Api.Controllers.Organizations;

[Authorize]
public class DeleteOrganizationEndpoint : BaseApiController
{
    private readonly IOrganizationService _organizationService;

    public DeleteOrganizationEndpoint(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpDelete("organizations/{id}")]
    public async Task<IActionResult> UpdateOrganization(int id, CancellationToken ct)
    {
        try
        {
            await _organizationService.DeleteAsync(id, ct);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(_response.FailureResponse(ex.Message));
        }
    }
}
