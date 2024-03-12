using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Experiences;

[Authorize]
public class AddExperiencEndpoint(IExperienceService experienceService) : BaseApiController
{
    [Tags("profiles/experiences")]
    [HttpPost("profiles/experiences")]
    public async Task<IActionResult> Experience(AddExperienceRequest request, CancellationToken ct)
    {
        try
        {
            await experienceService.AddAsync(request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}