using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests;

using TeamsMaker.Api.Services.Organizations;

namespace TeamsMaker.Api.Controllers;


[Authorize]
public class AddOrganizationEndpoint : BaseApiController
{
    private readonly IOrganizationService _organizationService;

    public AddOrganizationEndpoint(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [HttpPost("organizations")]
    public async Task<IActionResult> AddOrganization(AddOrganizationRequest request, CancellationToken ct)
    {
        await _organizationService.AddAsync(request, ct);

        return Created("", _response.SuccessResponse(null));
    }
}
