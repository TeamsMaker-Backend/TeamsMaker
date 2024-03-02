using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Profile;
using TeamsMaker.Api.Controllers.Files;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles;

[Authorize]
public class GetOtherProfileEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [Tags("profiles")]
    [HttpGet("profiles/{userEnum}/{id}")]
    public async Task<IActionResult> Profile(UserEnum userEnum, string id, CancellationToken ct)
    {
        var profileService = _serviceProvider.GetRequiredKeyedService<IProfileService>(userEnum);
        GetOtherProfileResponse response;

        try
        {
            response = await profileService.GetOtherProfileAsync(id, ct);
            LoadFiles(id, GetBaseType(userEnum), response);
        }
        catch (ArgumentException)
        {
            return NotFound(_response.FailureResponse("Invalid Data!"));
        }

        return Ok(_response.SuccessResponse(response));
    }

    private void LoadFiles(string id, string? baseType, GetOtherProfileResponse response)
    {
        response.Avatar = Url.Action(nameof(GetFileEndpoint.File), nameof(GetFileEndpoint), new { baseType, id, fileType = FileTypes.Avatar }, Request.Scheme);
        response.Header = Url.Action(nameof(GetFileEndpoint.File), nameof(GetFileEndpoint), new { baseType, id, fileType = FileTypes.Header }, Request.Scheme);

        if (response.OtherStudentInfo != null)
            response.OtherStudentInfo.CV = Url.Action(nameof(GetFileEndpoint.File), nameof(GetFileEndpoint), new { baseType, id, fileType = FileTypes.CV }, Request.Scheme);
    }

    private static string GetBaseType(UserEnum userEnum)
        => userEnum switch
        {
            UserEnum.Student => BaseTypes.Student,
            UserEnum.Staff => BaseTypes.Staff,
            _ => string.Empty,
        };
}
