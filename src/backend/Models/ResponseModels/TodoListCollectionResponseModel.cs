namespace EzraTodoApi.Models.ResponseModels;

public class TodoListCollectionResponseModel
{
    public List<TodoListResponseModel> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public DateTime LastSynced { get; set; }
}
