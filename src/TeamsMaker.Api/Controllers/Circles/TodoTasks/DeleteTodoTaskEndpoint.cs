using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Services.Circles.Interfaces;


namespace TeamsMaker.Api.Controllers.Circles.TodoTasks;

[Authorize]
public class DeleteTodoTaskEndpoint(ITodoTaskService todoTaskService) : BaseApiController
{
    [Tags("circles/todo_tasks")]
    [HttpDelete("circles/todo_tasks/{id}")]
    public async Task<IActionResult> TodoTask(Guid id, CancellationToken ct)
    {
        try
        {
            await todoTaskService.DeleteAsync(id, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created("", _response.SuccessResponse(null));
    }
}
