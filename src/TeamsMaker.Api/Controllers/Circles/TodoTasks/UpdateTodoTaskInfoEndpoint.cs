using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.TodoTask;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.TodoTasks;

[Authorize]
public class UpdateTodoTaskInfoEndpoint(ITodoTaskService todoTaskService) : BaseApiController
{
    [Tags("circles/todo_tasks")]
    [HttpPatch("circles/todo_tasks/{id}")]
    public async Task<IActionResult> TodoTask(Guid id, [FromBody] UpdateTodoTaskInfoRequest request, CancellationToken ct)
    {
        try
        {
            await todoTaskService.UpdateInfoAsync(id, request, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
