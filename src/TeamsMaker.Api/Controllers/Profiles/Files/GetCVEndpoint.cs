using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Files;

public class GetCVEndpoint(IProfileService profileService) : BaseApiController
{
    private readonly IProfileService _profileService = profileService;

    [HttpGet("profiles/cvs/{id}")]
    public async Task<IActionResult> CV(Guid id, CancellationToken ct)
    {
        FileContentResult? result;
        try
        {
            result = await _profileService.GetCVAsync(id, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }

        return Ok(_response.SuccessResponse(result));
    }
}
