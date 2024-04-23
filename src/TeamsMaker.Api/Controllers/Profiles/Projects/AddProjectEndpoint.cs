using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class AddProjectEndpoint(IProjectService projectService) : BaseApiController
{
    [Tags("profiles/projects")]
    [HttpPost("profiles/projects")]
    public async Task<IActionResult> Project(AddProjectRequest request, CancellationToken ct)
    {
        int id;

        try
        {
            id = await projectService.AddAsync(request, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created("", _response.SuccessResponse(new { id }));
    }
}
