using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class UpdateProjectEndpoint(IStudentProfileService studentProfileService) : BaseApiController
{
    private readonly IStudentProfileService _studentProfileService = studentProfileService;

    [HttpPut("profiles/projects/{id}")]
    public async Task<IActionResult> Project(int id, ProjectRequest request, CancellationToken ct)
    {
        try
        {
            await _studentProfileService.UpdateProjectAsync(id, request, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }
        return Ok();
    }
}
