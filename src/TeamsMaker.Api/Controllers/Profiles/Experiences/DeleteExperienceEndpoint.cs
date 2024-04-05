using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Experiences;

[Authorize]
public class DeleteExperienceEndpoint(IExperienceService experienceService) : BaseApiController
{
    [Tags("profiles/experiences")]
    [HttpDelete("profiles/experiences/{id}")]
    public async Task<IActionResult> Experience(int id, CancellationToken ct)
    {
        try
        {
            await experienceService.DeleteAsync(id, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}