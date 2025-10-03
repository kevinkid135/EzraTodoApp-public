using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzraTodoApi.Models.RequestModels;

public class UpdateTodoItemRequestModel
{
    [MaxLength(255)]
    public string? Title { get; set; }

    public bool? IsCompleted { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DueDate { get; set; }
}