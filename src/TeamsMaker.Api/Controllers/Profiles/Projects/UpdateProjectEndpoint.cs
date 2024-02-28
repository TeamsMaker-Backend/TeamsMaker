using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Projects;

[Authorize]
public class UpdateProjectEndpoint(IProjectService projectService) : BaseApiController
{
    private readonly IProjectService _projectService = projectService;

    [Tags("profiles/Projects")]
    [HttpPut("profiles/projects/{id}")]
    public async Task<IActionResult> Project(int id, ProjectRequest request, CancellationToken ct)
    {
        try
        {
            await _projectService.UpdateProjectAsync(id, request, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }
        return Ok();
    }
}
