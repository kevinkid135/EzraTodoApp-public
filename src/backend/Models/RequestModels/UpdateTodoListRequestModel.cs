namespace EzraTodoApi.Models.RequestModels;

public class UpdateTodoListRequestModel
{
    public string? Name { get; set; }
    public bool? IsDeleted { get; set; }
}
