using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;

using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Experiences;

[Authorize]
public class UpdateExperienceEndpoint(IExperienceService experienceService) : BaseApiController
{
    [Tags("profiles/experiences")]
    [HttpPut("profiles/experiences/{id}")]
    public async Task<IActionResult> Experience(int id, [FromBody] UpdateExperienceRequest updateExperienceRequest, CancellationToken ct)
    {
        try
        {
            await experienceService.UpdateAsync(id, updateExperienceRequest, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
