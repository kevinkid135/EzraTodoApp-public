using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzraTodoApi.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "TodoLists",
            columns: table => new
            {
                TodoListId = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TodoLists", x => x.TodoListId);
            });

        migrationBuilder.CreateTable(
            name: "TodoItems",
            columns: table => new
            {
                TodoItemId = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                TodoListId = table.Column<int>(type: "INTEGER", nullable: false),
                Title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TodoItems", x => x.TodoItemId);
                table.ForeignKey(
                    name: "FK_TodoItems_TodoLists_TodoListId",
                    column: x => x.TodoListId,
                    principalTable: "TodoLists",
                    principalColumn: "TodoListId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_TodoItems_TodoListId",
            table: "TodoItems",
            column: "TodoListId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "TodoItems");

        migrationBuilder.DropTable(
            name: "TodoLists");
    }
}
