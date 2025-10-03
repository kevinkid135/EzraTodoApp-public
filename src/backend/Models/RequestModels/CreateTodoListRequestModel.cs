namespace EzraTodoApi.Models.RequestModels;

public class CreateTodoListRequestModel
{
    public required string Name { get; set; }
    public Guid OwnerId { get; set; }
}