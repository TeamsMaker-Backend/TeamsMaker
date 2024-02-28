using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class AddExperiencEndpoint(IStudentProfileService studentProfileService) : BaseApiController
{
    private readonly IStudentProfileService _studentProfileService = studentProfileService;

    [HttpPost("profiles/experiences")]
    public async Task<IActionResult> Experience(ExperienceRequest request, CancellationToken ct)
    {
        try
        {
            await _studentProfileService.AddExperienceAsync(request, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }
        return Ok();
    }
}