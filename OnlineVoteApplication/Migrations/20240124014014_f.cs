using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineVoteApplication.Migrations
{
    public partial class f : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VotingSystems",
                table: "VotingSystems");

            migrationBuilder.RenameTable(
                name: "VotingSystems",
                newName: "VotingSystem");

            migrationBuilder.AddColumn<string>(
                name: "IDProofImage",
                table: "Voters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VotingSystem",
                table: "VotingSystem",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VotingSystem",
                table: "VotingSystem");

            migrationBuilder.DropColumn(
                name: "IDProofImage",
                table: "Voters");

            migrationBuilder.RenameTable(
                name: "VotingSystem",
                newName: "VotingSystems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VotingSystems",
                table: "VotingSystems",
                column: "Id");
        }
    }
}
