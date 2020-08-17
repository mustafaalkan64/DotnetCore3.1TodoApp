using Microsoft.EntityFrameworkCore.Migrations;

namespace Todo_App.DAL.Migrations
{
    public partial class TodoStatusFieldAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "tbl_TodoList",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "tbl_TodoList");
        }
    }
}
