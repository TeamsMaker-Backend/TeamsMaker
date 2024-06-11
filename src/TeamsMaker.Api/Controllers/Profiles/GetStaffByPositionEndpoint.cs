using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Core.Enums;
using TeamsMaker.Api.Services.Profiles.Interfaces;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.Controllers.Profiles;

public class GetStaffByPositionEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    [HttpGet("profiles/staff")]
    public async Task<IActionResult> GetStaffLookups([FromQuery] PositionEnum position, CancellationToken ct)
    {
        var staffProfileService = serviceProvider.GetRequiredKeyedService<IProfileService>(UserEnum.Staff);

        try
        {
            var lookups = await staffProfileService.GetStaffLookupsAsync(position, ct);
            
            return Ok(_response.SuccessResponse(lookups));
        }
        catch (Exception ex)
        { 
            return NotFound(_response.FailureResponse(ex.Message));
        }
    }
}
