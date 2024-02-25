using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Files;

public class GetAvatarEndpoint(IProfileService profileService) : BaseApiController
{
    private readonly IProfileService _profileService = profileService;

    [HttpGet("profiles/avatars/{id}")]
    public async Task<IActionResult> Avatar(Guid id, CancellationToken ct)
    {
        FileContentResult? result;
        try
        {
            result = await _profileService.GetAvatarAsync(id, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }

        return Ok(_response.SuccessResponse(result));
    }
}
