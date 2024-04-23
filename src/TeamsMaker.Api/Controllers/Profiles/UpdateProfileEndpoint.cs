using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Profile;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class UpdateProfileEndpoint(IServiceProvider serviceProvider, IUserInfo userInfo) : BaseApiController
{
    [Tags("profiles")]
    [HttpPut("profiles")]
    public async Task<IActionResult> Profile(UpdateProfileRequest request, CancellationToken ct)
    {
        var profileService = serviceProvider.GetRequiredKeyedService<IProfileService>(GetKey());

        try
        {
            await profileService.UpdateAsync(request, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }

    private UserEnum GetKey() // Refactor
        => userInfo.Roles.Contains(AppRoles.Student) ? UserEnum.Student : UserEnum.Staff;
}
