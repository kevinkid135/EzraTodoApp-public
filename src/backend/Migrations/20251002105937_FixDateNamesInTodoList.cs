using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EzraTodoApi.Migrations
{
    /// <inheritdoc />
    public partial class FixDateNamesInTodoList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "TodoLists",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "TodoLists",
                newName: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "TodoLists",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "TodoLists",
                newName: "CreateDate");
        }
    }
}
