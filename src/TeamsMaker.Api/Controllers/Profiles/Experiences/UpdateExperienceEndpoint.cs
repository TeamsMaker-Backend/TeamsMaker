using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Experiences;

[Authorize]
public class UpdateExperienceEndpoint(IExperienceService experienceService) : BaseApiController
{
    private readonly IExperienceService _experienceService = experienceService;

    [Tags("profiles/experiences")]
    [HttpPut("profiles/experiences/{id}")]
    public async Task<IActionResult> Experience(int id, ExperienceRequest request, CancellationToken ct)
    {
        try
        {
            await _experienceService.UpdateExperienceAsync(id, request, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }
        return Ok();
    }
}
