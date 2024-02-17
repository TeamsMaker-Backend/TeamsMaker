using Microsoft.AspNetCore.Authorization;
using TeamsMaker.Api.Services.Organizations;
using Microsoft.AspNetCore.Mvc;


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
        await _organizationService.DeleteAsync(id, ct);

        return NoContent();
    }
}
