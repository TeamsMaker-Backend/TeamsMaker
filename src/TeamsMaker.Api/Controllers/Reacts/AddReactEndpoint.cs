using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Services.Reacts.Interfaces;

namespace TeamsMaker.Api.Controllers.Reacts;

[Authorize]
public class AddReactEndpoint(IReactService reactService) : BaseApiController
{
    [Tags("React")]
    [Produces<IdResponse<Guid>>]
    [HttpPost("react")]
    public async Task<IActionResult> AddReact ([FromQuery] Guid id, CancellationToken ct)
    {
        Guid reactId;
        try
        {
            reactId = await reactService.AddAsync(id, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
        return Created("", _response.SuccessResponse(new IdResponse<Guid>(reactId)));
    }
}
