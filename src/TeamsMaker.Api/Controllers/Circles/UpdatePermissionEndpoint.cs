using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.Circle;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles;

[Authorize]
public class UpdatePermissionEndpoint(IServiceProvider serviceProvider) : BaseApiController
{
    [Tags("circles/permissions/danger_zone")]
    [HttpPut("circles/permissions/{baseType}/{entityId}")]
    public async Task<IActionResult> Permission(string baseType, Guid entityId, UpdatePermissionRequest? request, CancellationToken ct)
    {
        var permissionService = serviceProvider.GetRequiredKeyedService<IPermissionService>(baseType);

        try
        {
            await permissionService.UpdatePermissionAsync(entityId, request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok();
    }
}
