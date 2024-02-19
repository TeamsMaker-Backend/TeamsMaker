using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Services.ProfileService.Interface;

namespace TeamsMaker.Api.Controllers.Profile;

public class GetProfileEndpoint : BaseApiController
{
    private readonly IProfileService _profileService;

    public GetProfileEndpoint(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("profiles/{email}")]
    public async Task<IActionResult> Profile(string email, CancellationToken ct)
    {
        ProfileResponse profile;
        try
        {
            profile = await _profileService.GetProfileAsync(email, ct);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid User Email"));
        }
        return Ok(_response.SuccessResponse(profile));
    }
}
