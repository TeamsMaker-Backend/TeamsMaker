using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Services.Reacts;
using TeamsMaker.Api.Services.Reacts.Interfaces;

namespace TeamsMaker.Api.Controllers.Reacts;

[Authorize]
public class DeleteReactEndpoint(IReactService reactService) : BaseApiController
{

    [Tags("React")]
    [HttpDelete("react")]
    public async Task<IActionResult> DeleteReact([FromQuery] Guid id, CancellationToken ct)
    {
        try
        {
            await reactService.DeleteAsync(id, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Ok(_response.SuccessResponse(null));
    }
}
