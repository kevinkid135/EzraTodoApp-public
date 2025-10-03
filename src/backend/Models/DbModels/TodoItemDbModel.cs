using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzraTodoApi.Models.DbModels;

[Table("TodoItems")]
public class TodoItemDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TodoItemId { get; set; }

    [Required]
    public int TodoListId { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Title { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [ForeignKey(nameof(TodoListId))]
    public TodoListDbModel TodoList { get; set; } = null!;
}