using EzraTodoApi.Manager;
using EzraTodoApi.Models.RequestModels;
using System.Threading.Tasks;

namespace EzraTodoApi.Handler;

public class TodoItemHandler
{
    private readonly ILogger<TodoItemHandler> _logger;
    private readonly ITodoItemManager _todoItemManager;

    public TodoItemHandler(ILogger<TodoItemHandler> logger, ITodoItemManager manager)
    {
        _logger = logger;
        _todoItemManager = manager;
    }

    internal async Task<IResult> CreateTodoItemAsync(HttpContext context, CreateTodoItemRequestModel model, int todolistid)
    {
        var result = await _todoItemManager.CreateTodoItemAsync(model, todolistid);
        if (result == null || result.IsFailed)
        {
            // Do not return error details to avoid leaking internal information or sensitive data
            return Results.InternalServerError();
        }
        return Results.Ok(result.Value);
    }

    internal async Task<IResult> GetAllTodoItemsAsync(int todolistid, bool? includeDeleted, DateTime? since)
    {
        var result = await _todoItemManager.GetTodoItemsAsync(todolistid, includeDeleted, since);
        if (result == null || result.IsFailed)
        {
            // Do not return error details to avoid leaking internal information or sensitive data
            return Results.InternalServerError();
        }
        return Results.Ok(result.Value);
    }

    internal async Task<IResult> UpdateTodoItemAsync(HttpContext context, int todolistid, int todoItemId, UpdateTodoItemRequestModel model)
    {
        var result = await _todoItemManager.UpdateTodoItemAsync(model, todolistid, todoItemId);
        if (result == null || result.IsFailed) // TODO figure out what 404 looks like
        {
            // Do not return error details to avoid leaking internal information or sensitive data
            return Results.InternalServerError();
        }
        return Results.Ok(result.Value);
    }
}