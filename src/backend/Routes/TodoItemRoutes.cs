using EzraTodoApi.Handler;
using EzraTodoApi.Models.RequestModels;
using EzraTodoApi.Models.ResponseModels; // Add for DTOs

namespace EzraTodoApi.Routes;

public static class TodoItemRoutes
{
    /// <summary>
    /// Maps routes for managing todo items.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    public static void MapTodoItemRoutes(this WebApplication app)
    {
        /// <summary>
        /// Creates a new todo item.
        /// </summary>
        /// <param name="todolistid">The ID of the todo list this item belongs to.</param>
        app.MapPost("/todolist/{todolistid}/todoitem",
            (
                HttpContext context,
                int todolistid,
                CreateTodoItemRequestModel model,
                TodoItemHandler handler
            ) => handler.CreateTodoItemAsync(context, model, todolistid))
            .Produces<TodoItemResponseModel>(StatusCodes.Status200OK);

        /// <summary>
        /// Gets all todo items in a list.
        /// </summary>
        /// <param name="todolistid">The ID of the todo list this item belongs to.</param>
        /// <param name="includeDeleted">Whether to include soft-deleted items in the response.</param>
        /// <param name="since">Fetch items updated since this timestamp (optional).</param>
        app.MapGet("/todolist/{todolistid}/todoitems",
            (
                HttpContext context,
                int todolistid,
                TodoItemHandler handler,
                bool? includeDeleted,
                DateTime? since
            ) => handler.GetAllTodoItemsAsync(todolistid, includeDeleted, since))
            .Produces<TodoItemCollectionResponseModel>(StatusCodes.Status200OK);

        /// <summary>
        /// Updates an existing todo item.
        /// </summary>
        /// <param name="todolistid">The ID of the todo list this item belongs to.</param>
        /// <param name="todoitemid">The ID of the todo item to update.</param>
        app.MapPut("/todolist/{todolistid}/todoitem/{todoitemid}",
            async (
                HttpContext context,
                int todolistid,
                int todoitemid,
                UpdateTodoItemRequestModel model,
                TodoItemHandler handler
            ) => await handler.UpdateTodoItemAsync(context, todolistid, todoitemid, model))
            .Produces<TodoItemResponseModel>(StatusCodes.Status200OK);
    }
}
