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
    public async Task<IActionResult> Project(AddProjectRequest request, CancellationToken ct)
    {
        try
        {
            await _studentProfileService.AddProjectAsync(request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}
