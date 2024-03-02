using DataAccess.Base.Interfaces;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Controllers.Files;
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
            LoadFiles(_userInfo.UserId, GetBaseType(GetKey()), response);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data!"));
        }

        return Ok(_response.SuccessResponse(response));
    }

    private void LoadFiles(string id, string baseType, GetProfileResponse response)
    {
        response.Avatar = Url.Action(nameof(GetFileEndpoint.File), nameof(GetFileEndpoint), new { baseType, id, fileType = FileTypes.Avatar }, Request.Scheme);
        response.Header = Url.Action(nameof(GetFileEndpoint.File), nameof(GetFileEndpoint), new { baseType, id, fileType = FileTypes.Header }, Request.Scheme);

        if (response.StudentInfo != null)
            response.StudentInfo.CV = Url.Action(nameof(GetFileEndpoint.File), nameof(GetFileEndpoint), new { baseType, id, fileType = FileTypes.CV }, Request.Scheme);
    }

    private UserEnum GetKey()
        => _userInfo.Roles.Contains(AppRoles.Student) ? UserEnum.Student : UserEnum.Staff;

    private static string GetBaseType(UserEnum userEnum)
        => userEnum switch
        {
            UserEnum.Student => BaseTypes.Student,
            UserEnum.Staff => BaseTypes.Staff,
            _ => string.Empty,
        };
}
