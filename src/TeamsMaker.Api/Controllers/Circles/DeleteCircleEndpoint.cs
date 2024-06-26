﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class DeleteCircleEndpoint(ICircleService circleService) : BaseApiController
{
    [Tags("circles/danger_zone")]
    [HttpDelete("circles/{circleId}")]
    public async Task<IActionResult> Circle(Guid circleId, CancellationToken ct)
    {
        try
        {
            await circleService.DeleteAsync(circleId, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
