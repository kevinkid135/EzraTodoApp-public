using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzraTodoApi.Models.DbModels;

[Table("TodoLists")]
public class TodoListDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TodoListId { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public Guid OwnerId { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    public DateTime UpdatedDate { get; set; }

    public ICollection<TodoItemDbModel> TodoItems { get; set; } = new List<TodoItemDbModel>();
}