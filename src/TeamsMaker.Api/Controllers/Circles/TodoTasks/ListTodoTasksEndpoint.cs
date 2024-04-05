using Core.Generics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.QueryStringParameters;
using TeamsMaker.Api.Contracts.Responses.TodoTask;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.TodoTasks;

[Authorize]
public class ListTodoTasksEndpoint(ITodoTaskService todoTaskService) : BaseApiController
{
    [Tags("circles/todo_tasks")]
    [Produces(typeof(PagedList<GetTodoTaskResponse>))]
    [HttpGet("circles/{id}/todo_tasks")]
    public async Task<IActionResult> TodoTasks(Guid id, [FromQuery] TodoTaskQueryString queryString, CancellationToken ct)
    {
        try
        {
            var todoTasks = await todoTaskService.ListAsync(id, queryString, ct);
            return todoTasks is not null ? Ok(_response.SuccessResponseWithPagination(todoTasks)) : NotFound();
        }
        catch (ArgumentException e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }
    }
}
