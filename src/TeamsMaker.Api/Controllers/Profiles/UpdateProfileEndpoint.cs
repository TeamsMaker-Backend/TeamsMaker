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
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IUserInfo _userInfo = userInfo;
    private IProfileService? _profileService;

    [HttpPut("profiles")]
    public async Task<IActionResult> Profile([FromForm] UpdateProfileRequest request, CancellationToken ct)
    {
        _profileService = _serviceProvider.GetRequiredKeyedService<IProfileService>(GetKey());

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

    private UserEnum GetKey() // Refactor
        => _userInfo.Roles.Contains(AppRoles.Student) ? UserEnum.Student : UserEnum.Staff;
}
