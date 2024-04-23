using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class GetOtherProfileEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    [Tags("profiles")]
    [Produces<GetOtherProfileResponse>]
    [HttpGet("profiles/{userEnum}/{id}")]
    public async Task<IActionResult> Profile(UserEnum userEnum, string id, CancellationToken ct)
    {
        var profileService = serviceProvider.GetRequiredKeyedService<IProfileService>(userEnum);
        GetOtherProfileResponse response;

        try
        {
            response = await profileService.GetOtherAsync(id, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }
}
