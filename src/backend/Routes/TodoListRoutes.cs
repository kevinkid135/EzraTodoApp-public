using EzraTodoApi.Handler;
using EzraTodoApi.Models.RequestModels;
using EzraTodoApi.Models.ResponseModels; // Add for DTOs

namespace EzraTodoApi.Routes;

public static class TodoListRoutes
{
    /// <summary>
    /// Maps routes for managing todo lists.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    public static void MapTodoListRoutes(this WebApplication app)
    {
        /// <summary>
        /// Creates a new todo list.
        /// </summary>
        app.MapPost("/todolist",
            (
                HttpContext context,
                CreateTodoListRequestModel model,
                TodoListHandler handler
            ) => handler.CreateTodoListAsync(context, model))
            .Produces<TodoListResponseModel>(StatusCodes.Status200OK);

        /// <summary>
        /// Gets all todo lists.
        /// </summary>
        /// <param name="includeDeleted">Whether to include soft-deleted lists in the response.</param>
        app.MapGet("/todolist", 
            (
                HttpContext context, 
                TodoListHandler handler,
                bool? includeDeleted
            ) => handler.GetAllTodoListsAsync(includeDeleted))
            .Produces<TodoListCollectionResponseModel>(StatusCodes.Status200OK);

        /// <summary>
        /// Updates a todo list.
        /// </summary>
        /// <param name="listid">The ID of the todo list to update.</param>
        app.MapPut("/todolist/{listid}",
            (
                HttpContext context,
                int listid,
                UpdateTodoListRequestModel model,
                TodoListHandler handler
            ) => handler.UpdateTodoListAsync(context, listid, model))
            .Produces<TodoListResponseModel>(StatusCodes.Status200OK);
    }
}
