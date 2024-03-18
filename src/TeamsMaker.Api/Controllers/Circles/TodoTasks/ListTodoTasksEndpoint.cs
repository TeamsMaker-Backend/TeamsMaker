using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.TodoTasks;

[Authorize]
public class ListTodoTasksEndpoint(ITodoTaskService todoTaskService) : BaseApiController
{
    [Tags("circles/todo_tasks")]
    [HttpGet("circles/{id}/todo_tasks/{status}")]
    public async Task<IActionResult> TodoTasks(Guid id, TodoTaskStatus status, [FromQuery] TodoTaskQueryString queryString, CancellationToken ct)
    {
        try
        {
            var todoTasks = await todoTaskService.ListAsync(id, status, queryString, ct);
            return todoTasks is not null ? Ok(_response.SuccessResponse(todoTasks)) : NotFound();
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}
