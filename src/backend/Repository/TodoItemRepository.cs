using EzraTodoApi.Models;
using EzraTodoApi.Models.DbModels;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace EzraTodoApi.Repository;

public interface ITodoItemRepository
{
    Task<Result<TodoItemDbModel>> CreateTodoItemAsync(TodoItemDbModel model);
    Task<Result<List<TodoItemDbModel>>> GetTodoItemsAsync(int listId, bool? includeDeleted, DateTime? since);
    Task<Result<TodoItemDbModel>> GetTodoItemAsync(int listId, int todoItemId);
    Task<Result<TodoItemDbModel>> UpdateTodoItemAsync(TodoItemDbModel existingItem);
}

public class TodoItemRepository : ITodoItemRepository
{
    private readonly ILogger<TodoItemRepository> _logger;
    private readonly EzraTodoDbContext _dbContext;

    public TodoItemRepository(ILogger<TodoItemRepository> logger, EzraTodoDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result<TodoItemDbModel>> CreateTodoItemAsync(TodoItemDbModel model)
    {
        try
        {
            _dbContext.TodoItems.Add(model);
            await _dbContext.SaveChangesAsync();
            return Result.Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating todo Item");
            return Result.Fail<TodoItemDbModel>(ex.Message);
        }
    }

    public async Task<Result<List<TodoItemDbModel>>> GetTodoItemsAsync(int listId, bool? includeDeleted, DateTime? since)
    {
        try
        {
            var query = _dbContext.TodoItems
                .Where(item => item.TodoListId == listId);

            if (includeDeleted is not true)
            {
                query = query.Where(item => !item.IsDeleted);
            }

            if (since.HasValue)
            {
                query = query.Where(item => item.UpdatedDate >= since.Value);
            }

            var todoItems = await query.ToListAsync();
            return Result.Ok(todoItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todo items for list ID {ListId}", listId);
            return Result.Fail<List<TodoItemDbModel>>(ex.Message);
        }
    }

    public async Task<Result<TodoItemDbModel>> GetTodoItemAsync(int listId, int todoItemId)
    {
        try
        {
            var item = await _dbContext.TodoItems
                .Where(i => i.TodoListId == listId && i.TodoItemId == todoItemId)
                .FirstOrDefaultAsync();

            return item == null ? Result.Fail<TodoItemDbModel>("Todo item not found") : Result.Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todo item {TodoItemId} for list ID {ListId}", todoItemId, listId);
            return Result.Fail<TodoItemDbModel>(ex.Message);
        }
    }

    public async Task<Result<TodoItemDbModel>> UpdateTodoItemAsync(TodoItemDbModel model)
    {
        try
        {
            _dbContext.TodoItems.Update(model);
            await _dbContext.SaveChangesAsync();
            return Result.Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating todo item {TodoItemId}", model.TodoItemId);
            return Result.Fail<TodoItemDbModel>(ex.Message);
        }
    }
}