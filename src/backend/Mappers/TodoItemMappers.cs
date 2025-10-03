using EzraTodoApi.Models.DbModels;
using EzraTodoApi.Models.RequestModels;
using EzraTodoApi.Models.ResponseModels;

namespace EzraTodoApi.Mappers;

public static class TodoItemMappers
{
    public static TodoItemDbModel ToDbModel(this CreateTodoItemRequestModel requestModel, int listId)
    {
        var currentDate = DateTime.UtcNow;
        return new TodoItemDbModel
        {
            Title = requestModel.Title,
            TodoListId = listId,
            IsCompleted = false,
            IsDeleted = false,
            DueDate = requestModel.DueDate,
            CreatedDate = currentDate,
            UpdatedDate = currentDate,
        };
    }

    public static TodoItemResponseModel ToResponseModel(this TodoItemDbModel dbModel)
    {
        return new TodoItemResponseModel
        {
            Id = dbModel.TodoItemId,
            TodoListId = dbModel.TodoListId,
            Title = dbModel.Title,
            IsCompleted = dbModel.IsCompleted,
            IsDeleted = dbModel.IsDeleted,
            DueDate = dbModel.DueDate,
            CreatedDate = dbModel.CreatedDate,
            UpdatedDate = dbModel.UpdatedDate,
        };
    }

    public static TodoItemCollectionResponseModel ToResponseModel(this List<TodoItemDbModel> dbModels)
    {
        return new TodoItemCollectionResponseModel
        {
            Items = dbModels.Select(d => d.ToResponseModel()).ToList(),
            TotalCount = dbModels.Count,
            LastSynced = DateTime.UtcNow,
        };
    }
}
