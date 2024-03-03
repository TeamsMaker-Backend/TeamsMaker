using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Experiences;

[Authorize]
public class DeleteExperienceEndpoint(IExperienceService experienceService) : BaseApiController
{
    private readonly IExperienceService _experienceService = experienceService;

    [Tags("profiles/experiences")]
    [HttpDelete("profiles/experiences/{id}")]
    public async Task<IActionResult> Experience(int id, CancellationToken ct)
    {
        try
        {
            await _experienceService.DeleteExperienceAsync(id, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}