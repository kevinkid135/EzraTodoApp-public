namespace EzraTodoApi.Models.ResponseModels;

public class TodoListResponseModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public Guid OwnerId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}