namespace EzraTodoApi.Models.ResponseModels;

public class TodoItemCollectionResponseModel
{
    public List<TodoItemResponseModel> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public DateTime LastSynced { get; set; }
}
