﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class GetCircleMembersEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles")]
    [HttpGet("circles/members/{id}")]
    public async Task<IActionResult> CircleMember(Guid id, CancellationToken ct)
    {
        GetCircleMembersResponse response;

        try
        {
            response = await circleService.GetMembersAsync(id, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(response));
    }
}