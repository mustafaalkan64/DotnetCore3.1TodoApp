using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Todo_App.DAL.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    UserName = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 150, nullable: true),
                    Hash = table.Column<string>(maxLength: 250, nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_TodoList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    Desc = table.Column<string>(maxLength: 500, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    UpdateDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_TodoList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_TodoList_tbl_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_TodoList_tbl_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "tbl_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TodoList_CreatedBy",
                table: "tbl_TodoList",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TodoList_UpdatedBy",
                table: "tbl_TodoList",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_TodoList");

            migrationBuilder.DropTable(
                name: "tbl_Users");
        }
    }
}
