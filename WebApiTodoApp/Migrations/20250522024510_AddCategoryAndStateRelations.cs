using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiTodoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAndStateRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Assignments",
                newName: "StateId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dateCreation = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameState = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CategoryId",
                table: "Assignments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_StateId",
                table: "Assignments",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Categories_CategoryId",
                table: "Assignments",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_States_StateId",
                table: "Assignments",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Categories_CategoryId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_States_StateId",
                table: "Assignments");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_CategoryId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_StateId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "Assignments",
                newName: "Status");
        }
    }
}
