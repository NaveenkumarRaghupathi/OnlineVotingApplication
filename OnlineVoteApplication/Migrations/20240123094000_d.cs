using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVoteApplication.Migrations
{
    public partial class d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "ContestManagement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContestentImage",
                table: "ContestManagement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "ContestManagement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "ContestManagement",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "ContestManagement");

            migrationBuilder.DropColumn(
                name: "ContestentImage",
                table: "ContestManagement");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ContestManagement");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "ContestManagement");
        }
    }
}
