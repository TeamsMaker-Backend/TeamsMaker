using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;

using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Experiences;

[Authorize]
public class UpdateExperienceEndpoint(IExperienceService experienceService) : BaseApiController
{
    private readonly IExperienceService _experienceService = experienceService;

    [Tags("profiles/experiences")]
    [HttpPatch("profiles/experiences/{id}")]
    public async Task<IActionResult> Experience(int id, [FromBody] UpdateExperienceRequest updateExperienceRequest, CancellationToken ct)
    {
        try
        {
            await _experienceService.UpdateExperienceAsync(id, updateExperienceRequest
            , ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}
