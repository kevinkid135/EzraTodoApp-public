using EzraTodoApi.Mappers;
using EzraTodoApi.Models.DbModels;
using EzraTodoApi.Models.RequestModels;
using EzraTodoApi.Models.ResponseModels;
using EzraTodoApi.Repository;
using FluentResults;
using System.Collections.Generic;

namespace EzraTodoApi.Manager;

public interface ITodoListManager
{
    ValueTask<Result<TodoListResponseModel>> CreateTodoListAsync(CreateTodoListRequestModel model);
    ValueTask<Result<TodoListCollectionResponseModel>> GetAllTodoList(bool? includeDeleted);
    ValueTask<Result> DeleteTodoListAsync(int id);
    ValueTask<Result<TodoListResponseModel>> UpdateTodoListAsync(UpdateTodoListRequestModel model, int id);
}

public class TodoListManager : ITodoListManager
{
    private readonly ILogger<TodoListManager> _logger;
    private readonly ITodoListRepository _todoListRepository;

    public TodoListManager(ILogger<TodoListManager> logger, ITodoListRepository todoListRepository)
    {
        _logger = logger;
        _todoListRepository = todoListRepository;
    }

    public async ValueTask<Result<TodoListResponseModel>> CreateTodoListAsync(CreateTodoListRequestModel model)
    {
        TodoListDbModel dbModel;
        try
        {
            dbModel = model.ToDbModel();
        }
        catch
        {
            _logger.LogError("Failed to map requestModel to dbmodel");
            return Result.Fail("Failed to map requestModel to dbmodel");
        }

        var result = await _todoListRepository.CreateTodoListAsync(dbModel);
        if (result is null)
        {
            _logger.LogError("Failed to create todo list item");
            return Result.Fail("Failed to create todo list item");
        }
        else if (result.IsFailed)
        {
            _logger.LogError("Failed to create todo list item: {Errors}", string.Join(", ", result.Errors.Select(e => e.Message)));
            return Result.Fail("Failed to create todo list item");
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

    public async ValueTask<Result<TodoListCollectionResponseModel>> GetAllTodoList(bool? includeDeleted)
    {
        var result = await _todoListRepository.GetTodoListsAsync(includeDeleted);
        if (result is null)
        {
            _logger.LogError("Failed to get todo lists");
            return Result.Fail("Failed to create todo lists");
        }
        else if (result.IsFailed)
        {
            _logger.LogError("Failed to create todo lists: {Errors}", string.Join(", ", result.Errors.Select(e => e.Message)));
            return Result.Fail("Failed to create todo lists");
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

    public ValueTask<Result> DeleteTodoListAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Result<TodoListResponseModel>> UpdateTodoListAsync(UpdateTodoListRequestModel model, int id)
    {
        var existingResult = await _todoListRepository.GetTodoListAsync(id);
        if (existingResult is null || existingResult.IsFailed)
        {
            _logger.LogError("Todo List not found for update");
            return Result.Fail("Todo List not found");
        }

        var existingItem = existingResult.Value;

        // Update fields
        existingItem.Name = model.Name ?? existingItem.Name;
        existingItem.IsDeleted = model.IsDeleted ?? existingItem.IsDeleted;
        existingItem.UpdatedDate = DateTime.UtcNow;

        // Save changes
        var updateResult = await _todoListRepository.UpdateTodoListAsync(existingItem);
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
