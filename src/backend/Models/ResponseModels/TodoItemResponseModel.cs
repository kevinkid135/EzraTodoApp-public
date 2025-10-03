namespace EzraTodoApi.Models.ResponseModels;

public class TodoItemResponseModel
{
    public int Id { get; set; }
    public int TodoListId { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}