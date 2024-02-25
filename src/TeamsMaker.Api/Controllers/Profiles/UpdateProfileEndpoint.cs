using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class UpdateProfileEndpoint(IProfileService profileService) : BaseApiController
{
    private readonly IProfileService _profileService = profileService;

    [HttpPut("profiles")]
    public async Task<IActionResult> Profile([FromForm] UpdateProfileRequest request, CancellationToken ct)
    {
        try
        {
            await _profileService.UpdateProfileAsync(request, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }
        return Ok(_response.SuccessResponse(null));
    }
}
