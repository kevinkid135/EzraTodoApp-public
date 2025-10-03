using EzraTodoApi.Manager;
using EzraTodoApi.Models.RequestModels;
using FluentResults;

namespace EzraTodoApi.Handler;

public class TodoListHandler
{
    private readonly ILogger<TodoListHandler> _logger;
    private readonly ITodoListManager _todoListManager;

    public TodoListHandler(ILogger<TodoListHandler> logger, ITodoListManager manager)
    {
        _logger = logger;
        _todoListManager = manager;
    }

    public async Task<IResult> CreateTodoListAsync(HttpContext context, CreateTodoListRequestModel model)
    {
        var result = await _todoListManager.CreateTodoListAsync(model);
        if (result == null || result.IsFailed)
        {
            // Do not return error details to avoid leaking internal information or sensitive data
            return Results.InternalServerError();
        }
        return Results.Ok(result.Value);
    }

    public async Task<IResult> GetAllTodoListsAsync(bool? includeDeleted)
    {
        var result = await _todoListManager.GetAllTodoList(includeDeleted);
        if (result == null || result.IsFailed)
        {
            // Do not return error details to avoid leaking internal information or sensitive data
            return Results.InternalServerError();
        }
        return Results.Ok(result.Value);
    }

    public async Task<IResult> UpdateTodoListAsync(HttpContext context, int listId, UpdateTodoListRequestModel model)
    {
        var result = await _todoListManager.UpdateTodoListAsync(model, listId);
        if (result == null || result.IsFailed)
        {
            // Do not return error details to avoid leaking internal information or sensitive data
            return Results.InternalServerError();
        }
        return Results.Ok(result.Value);
    }
}