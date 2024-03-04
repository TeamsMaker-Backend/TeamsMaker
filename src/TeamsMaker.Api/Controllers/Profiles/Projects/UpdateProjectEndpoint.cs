using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Projects;

[Authorize]
public class UpdateProjectEndpoint(IProjectService projectService) : BaseApiController
{
    private readonly IProjectService _projectService = projectService;

    [Tags("profiles/projects")]
    [HttpPut("profiles/projects/{id}")]
    public async Task<IActionResult> Project(int id, ProjectRequest request, CancellationToken ct)
    {
        try
        {
            await _projectService.UpdateProjectAsync(id, request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok();
    }
}
