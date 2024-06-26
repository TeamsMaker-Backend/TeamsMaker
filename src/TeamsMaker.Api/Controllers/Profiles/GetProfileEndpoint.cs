﻿using DataAccess.Base.Interfaces;

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
    [Tags("profiles")]
    [Produces<GetProfileResponse>]
    [HttpGet("profiles/me")]
    public async Task<IActionResult> Profile(CancellationToken ct)
    {
        var profileService = serviceProvider.GetRequiredKeyedService<IProfileService>(GetKey());
        GetProfileResponse response;

        try
        {
            response = await profileService.GetAsync(ct);
        }
        catch (Exception ae)
        {
            return NotFound(_response.FailureResponse(ae.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }

    private UserEnum GetKey()
        => userInfo.Roles.Contains(AppRoles.Student) ? UserEnum.Student : UserEnum.Staff;
}
