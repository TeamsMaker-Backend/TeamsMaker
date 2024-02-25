using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Profiles.Interfaces;

namespace TeamsMaker.Api.Controllers.Profiles.Files;

public class GetHeaderEndpoint(IProfileService profileService) : BaseApiController
{
    private readonly IProfileService _profileService = profileService;

    [HttpGet("profiles/headers/{id}")]
    public async Task<IActionResult> Header(Guid id, CancellationToken ct)
    {
        FileContentResult? result;
        try
        {
            result = await _profileService.GetHeaderAsync(id, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data"));
        }

        return Ok(_response.SuccessResponse(result));
    }
}
