using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class GetProfileEndpoint(IServiceProvider serviceProvider, IUserInfo userInfo) : BaseApiController
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IUserInfo _userInfo = userInfo;

    [Tags("profiles")]
    [HttpGet("profiles/me")]
    public async Task<IActionResult> Profile(CancellationToken ct)
    {
        var profileService = _serviceProvider.GetRequiredKeyedService<IProfileService>(GetKey());
        GetProfileResponse response;

        try
        {
            response = await profileService.GetProfileAsync(ct);
        }
        catch (ArgumentException ae)
        {
            return NotFound(_response.FailureResponse(ae.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }

    private UserEnum GetKey()
        => _userInfo.Roles.Contains(AppRoles.Student) ? UserEnum.Student : UserEnum.Staff;
}
