using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Organizations;

namespace TeamsMaker.Api.Controllers;


[Authorize]
public class ListOrganizationsEndpoint : BaseApiController
{
    private readonly IOrganizationService _organizationService;

    public ListOrganizationsEndpoint(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }


    [HttpGet("organizations")]
    public async Task<IActionResult> ListOrganization(CancellationToken ct)
    {
        var orgs = await _organizationService.GetAsync(ct);

        return Ok(_response.SuccessResponse(orgs));
    }
}