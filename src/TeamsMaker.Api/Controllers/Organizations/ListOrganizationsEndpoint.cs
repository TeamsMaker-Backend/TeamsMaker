using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Services.Organizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TeamsMaker.Api.Controllers.Organizations;


[Authorize]
public class ListOrganizationsEndpoint : BaseApiController
{
    private readonly IOrganizationService _organizationService;

    public ListOrganizationsEndpoint(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }


    [HttpGet("organizations")]
    public async Task<IActionResult> ListOrganization([FromQuery] OrganizationsQueryString queryString, CancellationToken ct)
    {
        var orgs = await _organizationService.GetAsync(queryString, ct).ConfigureAwait(false);

        return Ok(_response.SuccessResponseWithPagination(orgs));
    }
}