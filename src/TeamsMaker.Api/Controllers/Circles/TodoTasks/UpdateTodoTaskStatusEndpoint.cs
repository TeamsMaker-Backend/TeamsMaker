using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.TodoTasks;

[Authorize]
public class UpdateTodoTaskStatusEndpoint(ITodoTaskService todoTaskService) : BaseApiController
{
    [Tags("circles/todo_tasks")]
    [HttpPatch("circles/todo_tasks/{id}/{status}")]
    public async Task<IActionResult> TodoTask(Guid id, [FromBody] TodoTaskStatus status, CancellationToken ct)
    {
        try
        {
            await todoTaskService.UpdateStatusAsync(id, status, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Ok(_response.SuccessResponse(null));
    }
}
