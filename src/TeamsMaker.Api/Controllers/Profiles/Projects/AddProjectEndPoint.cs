using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class AddProjectEndpoint(IProjectService projectService) : BaseApiController
{
    private readonly IProjectService _studentProfileService = projectService;

    [Tags("profiles/projects")]
    [HttpPost("profiles/projects")]
    public async Task<IActionResult> Project(ProjectRequest request, CancellationToken ct)
    {
        try
        {
            await _studentProfileService.AddProjectAsync(request, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }
        return Ok();
    }
}
