using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Controllers.Profiles.Files;
using TeamsMaker.Api.Services.ProfileService.Interface;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class GetProfileEndpoint(IProfileService profileService, IUserInfo userInfo) : BaseApiController
{
    private readonly IProfileService _profileService = profileService;
    private readonly IUserInfo _userInfo = userInfo;

    [HttpGet("profiles")]
    public async Task<IActionResult> Profile(CancellationToken ct)
    {
        GetProfileResponse response;
        try
        {
            response = await _profileService.GetProfileAsync(ct);
            LoadFiles(_userInfo.UserId, response);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data!"));
        }
        return Ok(_response.SuccessResponse(response));
    }

    private void LoadFiles(string id, GetProfileResponse response)
    {
        response.Avatar = Url.Action(nameof(GetAvatarEndpoint.Avatar), nameof(GetAvatarEndpoint), new { id }, Request.Scheme);
        response.Header = Url.Action(nameof(GetHeaderEndpoint.Header), nameof(GetHeaderEndpoint), new { id }, Request.Scheme);

        if (response.StudentInfo != null)
            response.StudentInfo!.CV = Url.Action(nameof(GetCVEndpoint.CV), nameof(GetCVEndpoint), new { id }, Request.Scheme);
    }
}
