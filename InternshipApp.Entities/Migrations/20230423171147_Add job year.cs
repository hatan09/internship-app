using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipApp.Core.Migrations
{
    public partial class Addjobyear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinYear",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinYear",
                table: "Jobs");
        }
    }
}
