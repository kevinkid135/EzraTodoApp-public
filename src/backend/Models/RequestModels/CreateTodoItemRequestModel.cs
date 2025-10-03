using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzraTodoApi.Models.RequestModels;

public class CreateTodoItemRequestModel
{
    [Required]
    [MaxLength(255)]
    public required string Title { get; set; }

    public DateTime? DueDate { get; set; }
}