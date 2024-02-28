using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class UpdateExperienceEndpoint(IStudentProfileService studentProfileService) : BaseApiController
{
    private readonly IStudentProfileService _studentProfileService = studentProfileService;

    [HttpPut("profiles/experiences/{id}")]
    public async Task<IActionResult> Experience(int id, ExperienceRequest request, CancellationToken ct)
    {
        try
        {
            await _studentProfileService.UpdateExperienceAsync(id, request, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }
        return Ok();
    }
}
