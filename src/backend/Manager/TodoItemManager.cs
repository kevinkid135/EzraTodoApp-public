using EzraTodoApi.Models.DbModels;
using EzraTodoApi.Models.RequestModels;
using EzraTodoApi.Repository;
using FluentResults;
using EzraTodoApi.Mappers;
using EzraTodoApi.Models.ResponseModels;

namespace EzraTodoApi.Manager;

public interface ITodoItemManager
{
    ValueTask<Result<TodoItemResponseModel>> CreateTodoItemAsync(CreateTodoItemRequestModel model, int listId);
    ValueTask<Result<TodoItemCollectionResponseModel>> GetTodoItemsAsync(int listId, bool? includeDeleted, DateTime? since);
    ValueTask<Result<TodoItemResponseModel>> UpdateTodoItemAsync(UpdateTodoItemRequestModel model, int listId, int todoItemId);
}

public class TodoItemManager : ITodoItemManager
{
    private readonly ILogger<TodoItemManager> _logger;
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoItemManager(ILogger<TodoItemManager> logger, ITodoItemRepository repository)
    {
        _logger = logger;
        _todoItemRepository = repository;
    }

    public async ValueTask<Result<TodoItemResponseModel>> CreateTodoItemAsync(CreateTodoItemRequestModel model, int listId)
    {
        TodoItemDbModel dbModel;
        try
        {
            dbModel = model.ToDbModel(listId);
        }
        catch
        {
            _logger.LogError("Failed to map requestModel to dbmodel");
            return Result.Fail("Failed to map requestModel to dbmodel");
        }

        var result = await _todoItemRepository.CreateTodoItemAsync(dbModel);
        if (result is null)
        {
            _logger.LogError("Failed to create todo item");
            return Result.Fail("Failed to create todo item");
        }
        else if (result.IsFailed)
        {
            _logger.LogError("Failed to create todo item: {Errors}", string.Join(", ", result.Errors.Select(e => e.Message)));
            return Result.Fail("Failed to create todo item");
        }
        var resultModel = result.Value;

        try
        {
            var responseModel = resultModel.ToResponseModel();
            return Result.Ok(responseModel);
        }
        catch
        {
            _logger.LogError("Failed to map dbmodel to responseModel");
            return Result.Fail("Failed to map dbmodel to responseModel");
        }
    }

    public async ValueTask<Result<TodoItemCollectionResponseModel>> GetTodoItemsAsync(int listId, bool? includeDeleted, DateTime? since)
    {
        var result = await _todoItemRepository.GetTodoItemsAsync(listId, includeDeleted, since);
        if (result is null)
        {
            _logger.LogError("Failed to get todo items");
            return Result.Fail("Failed to get todo items");
        }
        else if (result.IsFailed)
        {
            _logger.LogError("Failed to get todo items: {Errors}", string.Join(", ", result.Errors.Select(e => e.Message)));
            return Result.Fail("Failed to get todo items");
        }
        var resultModel = result.Value;

        try
        {
            var responseModel = resultModel.ToResponseModel();
            return Result.Ok(responseModel);
        }
        catch
        {
            _logger.LogError("Failed to map dbmodel to responseModel");
            return Result.Fail("Failed to map dbmodel to responseModel");
        }
    }

    public async ValueTask<Result<TodoItemResponseModel>> UpdateTodoItemAsync(UpdateTodoItemRequestModel model, int listId, int todoItemId)
    {
        var existingResult = await _todoItemRepository.GetTodoItemAsync(listId, todoItemId);
        if (existingResult is null || existingResult.IsFailed)
        {
            _logger.LogError("Todo item not found for update");
            return Result.Fail("Todo item not found");
        }

        var existingItem = existingResult.Value;

        // Update fields
        existingItem.Title = model.Title ?? existingItem.Title;
        existingItem.IsCompleted = model.IsCompleted ?? existingItem.IsCompleted;
        existingItem.IsDeleted = model.IsDeleted ?? existingItem.IsDeleted;
        existingItem.DueDate = model.DueDate ?? existingItem.DueDate;
        existingItem.UpdatedDate = DateTime.UtcNow;

        // Save changes
        var updateResult = await _todoItemRepository.UpdateTodoItemAsync(existingItem);
        if (updateResult is null || updateResult.IsFailed)
        {
            _logger.LogError("Failed to update todo item");
            return Result.Fail("Failed to update todo item");
        }

        try
        {
            var responseModel = updateResult.Value.ToResponseModel();
            return Result.Ok(responseModel);
        }
        catch
        {
            _logger.LogError("Failed to map dbmodel to responseModel");
            return Result.Fail("Failed to map dbmodel to responseModel");
        }
    }
}