using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class DeleteProjectEndpoint(IStudentProfileService studentProfileService) : BaseApiController
{
    private readonly IStudentProfileService _studentProfileService = studentProfileService;

    [HttpDelete("profiles/projects/{id}")]
    public async Task<IActionResult> Project(int id, CancellationToken ct)
    {
        try
        {
            await _studentProfileService.DeleteProjectAsync(id, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }
        return Ok();
    }
}