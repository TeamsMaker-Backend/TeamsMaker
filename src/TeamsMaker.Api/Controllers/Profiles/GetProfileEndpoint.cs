using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Controllers.Profiles.Files;
using TeamsMaker.Api.Services.ProfileService.Interface;

namespace TeamsMaker.Api.Controllers.Profiles;

public class GetProfileEndpoint(IProfileService profileService, IUserInfo userInfo) : BaseApiController
{
    private readonly IProfileService _profileService = profileService;
    private readonly IUserInfo _userInfo = userInfo;

    [HttpGet("profiles/{id}")]
    public async Task<IActionResult> Profile(Guid id, CancellationToken ct)
    {
        ProfileResponse response;
        try
        {
            response = await _profileService.GetProfileAsync(id, ct);

            response.Avatar = Url.Action(nameof(GetAvatarEndpoint.Avatar), nameof(GetAvatarEndpoint), new { id }, Request.Scheme);
            response.Header = Url.Action(nameof(GetHeaderEndpoint.Header), nameof(GetHeaderEndpoint), new { id }, Request.Scheme);

            if (response.StudentInfo != null)
                response.StudentInfo!.CV =
                    Url.Action(nameof(GetCVEndpoint.CV), nameof(GetCVEndpoint), new { id }, Request.Scheme);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data!"));
        }
        return Ok(_response.SuccessResponse(response));
    }
}
