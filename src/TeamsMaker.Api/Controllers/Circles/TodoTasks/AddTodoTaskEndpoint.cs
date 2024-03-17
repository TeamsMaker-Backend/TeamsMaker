using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.TodoTask;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.TodoTasks;

[Authorize]
public class AddTaskEndpoint(ITodoTaskService todoTaskService) : BaseApiController
{
    [Tags("circles/TodoTasks")]
    [HttpPost("circles/TodoTasks")]
    public async Task<IActionResult> TodoTask(AddTodoTaskRequest request, CancellationToken ct)
    {
        try
        {
            await todoTaskService.AddAsync(request, ct);
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created();
    }
}
