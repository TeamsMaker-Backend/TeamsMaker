using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Departments.Interfaces;

namespace TeamsMaker.Api.Controllers.Departments;

[Authorize]
public class ListDepatmentsAsLookupsEndpoint(IDepartmentService departmentService) : BaseApiController
{
    [HttpPost("api/departments/lookups")]
    public async Task<IActionResult> ListLookups(CancellationToken ct)
    {
        try
        {
            var depts = await departmentService.GetLookupsAync(ct);

            return Ok(_response.SuccessResponse(depts));
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}