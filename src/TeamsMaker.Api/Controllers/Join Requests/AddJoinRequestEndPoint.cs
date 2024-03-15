using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamsMaker.Api.Contracts.Requests.Join_Request;
using TeamsMaker.Api.Services.Join_Requests.Interfaces;

namespace TeamsMaker.Api.Controllers.Join_Requests
{
    [Authorize]
    public class AddJoinRequestEndPoint(IServiceProvider serviceProvider) : BaseApiController
    {
        [Tags("joinRequest")]
        [HttpPost("JoinRequest/{EntityType}")]
        public async Task<IActionResult> JoinRequest(AddJoinRequest request, string EntityType, CancellationToken ct)
        {
            var joinRequestService = serviceProvider.GetRequiredKeyedService<IJoinRequestService>(EntityType);

            try
            {
                await joinRequestService.AddJoinRequestAsync(request, ct);
            }
            catch (ArgumentException e)
            {
            return NotFound(_response.FailureResponse(e.Message));
            }

        return Ok();
    }

    }
}
