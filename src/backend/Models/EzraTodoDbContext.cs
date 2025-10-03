using EzraTodoApi.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace EzraTodoApi.Models;

public class EzraTodoDbContext : DbContext
{
    public EzraTodoDbContext(DbContextOptions<EzraTodoDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoListDbModel> TodoLists { get; set; }
    public DbSet<TodoItemDbModel> TodoItems { get; set; }

}
