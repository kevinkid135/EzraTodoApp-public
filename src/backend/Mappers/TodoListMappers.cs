using EzraTodoApi.Models.DbModels;
using EzraTodoApi.Models.RequestModels;
using EzraTodoApi.Models.ResponseModels;

namespace EzraTodoApi.Mappers;

public static class TodoListMappers
{
    public static TodoListDbModel ToDbModel(this CreateTodoListRequestModel responseModel)
    {
        var currentDate = DateTime.UtcNow;
        return new TodoListDbModel
        {
            Name = responseModel.Name,
            OwnerId = responseModel.OwnerId,
            IsDeleted = false,
            CreatedDate = currentDate,
            UpdatedDate = currentDate,
        };
    }

    public static TodoListResponseModel ToResponseModel(this TodoListDbModel dbModel)
    {
        return new TodoListResponseModel
        {
            Id = dbModel.TodoListId,
            Name = dbModel.Name,
            OwnerId = dbModel.OwnerId,
            IsDeleted = dbModel.IsDeleted,
            CreatedDate = dbModel.CreatedDate,
            UpdatedDate = dbModel.UpdatedDate,
        };
    }

    public static TodoListCollectionResponseModel ToResponseModel(this List<TodoListDbModel> dbModels)
    {
        return new TodoListCollectionResponseModel
        {
            Items = dbModels.Select(d => d.ToResponseModel()).ToList(),
            TotalCount = dbModels.Count,
            LastSynced = DateTime.UtcNow,
        };
    }
}
