using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class AddProjectEndpoint(IStudentProfileService studenrProfileService) : BaseApiController
{
    private readonly IStudentProfileService _studentProfileService = studenrProfileService;

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
