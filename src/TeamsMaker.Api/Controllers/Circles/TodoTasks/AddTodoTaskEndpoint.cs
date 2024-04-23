using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TeamsMaker.Api.Contracts.Requests.TodoTask;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Services.Circles.Interfaces;

namespace TeamsMaker.Api.Controllers.Circles.TodoTasks;

[Authorize]
public class AddTodoTaskEndpoint(ITodoTaskService todoTaskService) : BaseApiController
{
    [Tags("circles/todo_tasks")]
    [Produces<IdResponse<Guid>>]
    [HttpPost("circles/{circleId}/todo_tasks")]
    public async Task<IActionResult> TodoTask(Guid circleId, AddTodoTaskRequest request, CancellationToken ct)
    {
        Guid todoTaskId;

        try
        {
            todoTaskId = await todoTaskService.AddAsync(circleId, request, ct);
        }
        catch (Exception e)
        {
            return NotFound(_response.FailureResponse(e.Message));
        }

        return Created("", _response.SuccessResponse(new IdResponse<Guid>(todoTaskId)));
    }
}
