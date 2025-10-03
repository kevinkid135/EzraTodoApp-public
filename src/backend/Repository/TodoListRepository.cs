using EzraTodoApi.Models;
using EzraTodoApi.Models.DbModels;
using EzraTodoApi.Models.RequestModels;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EzraTodoApi.Repository;

public interface ITodoListRepository
{
    Task<Result<TodoListDbModel>> CreateTodoListAsync(TodoListDbModel model);
    Task<Result<TodoListDbModel>> GetTodoListAsync(int id);
    Task<Result<List<TodoListDbModel>>> GetTodoListsAsync(bool? includeDeleted);
    Task<Result<TodoListDbModel>> UpdateTodoListAsync(TodoListDbModel model);
}

public class TodoListRepository : ITodoListRepository
{
    private readonly ILogger<TodoListRepository> _logger;
    private readonly EzraTodoDbContext _dbContext;

    public TodoListRepository(ILogger<TodoListRepository> logger, EzraTodoDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<Result<TodoListDbModel>> CreateTodoListAsync(TodoListDbModel model)
    {
        try
        {
            _dbContext.TodoLists.Add(model);
            await _dbContext.SaveChangesAsync();
            return Result.Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating todo list");
            return Result.Fail<TodoListDbModel>(ex.Message);
        }
    }

    public async Task<Result<TodoListDbModel>> GetTodoListAsync(int id)
    {
        try
        {
            var todoList = await _dbContext.TodoLists
                .Where(i => i.TodoListId == id)
                .FirstOrDefaultAsync();

            return todoList == null ? Result.Fail<TodoListDbModel>("Todo list not found") : Result.Ok(todoList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todo lists");
            return Result.Fail<TodoListDbModel>(ex.Message);
        }
    }

    public async Task<Result<List<TodoListDbModel>>> GetTodoListsAsync(bool? includeDeleted)
    {
        try
        {
            IQueryable<TodoListDbModel> query = _dbContext.TodoLists;

            if (includeDeleted is not true)
            {
                query = query.Where(item => !item.IsDeleted);
            }

            var todoLists = await query.ToListAsync();
            return Result.Ok(todoLists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todo lists");
            return Result.Fail<List<TodoListDbModel>>(ex.Message);
        }
    }
    public async Task<Result<TodoListDbModel>> UpdateTodoListAsync(TodoListDbModel model)
    {
        try
        {
            _dbContext.TodoLists.Update(model);
            await _dbContext.SaveChangesAsync();
            return Result.Ok(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating todo item {TodoItemId}", model.TodoListId);
            return Result.Fail<TodoListDbModel>(ex.Message);
        }
    }
}