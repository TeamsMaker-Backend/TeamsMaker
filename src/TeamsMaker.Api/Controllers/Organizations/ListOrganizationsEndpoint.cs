using Core.Generics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Services.Organizations;

namespace TeamsMaker.Api.Controllers.Organizations;


[Authorize]
public class ListOrganizationsEndpoint(IOrganizationService organizationService) : BaseApiController
{
    [Tags("organizations")]
    [HttpGet("organizations")]
    public async Task<IActionResult> ListOrganization([FromQuery] OrganizationsQueryString queryString, CancellationToken ct)
    {
        PagedList<Contracts.Responses.GetOrganizationResponse> orgs;

        try
        {
            orgs = await organizationService.GetAsync(queryString, ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return BadRequest(_response.FailureResponse(ex.Message));
        }

        return Ok(_response.SuccessResponseWithPagination(orgs));
    }
}