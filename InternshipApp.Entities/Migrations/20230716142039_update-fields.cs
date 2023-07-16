using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipApp.Core.Migrations
{
    public partial class updatefields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceComment",
                table: "StudentForms");

            migrationBuilder.RenameColumn(
                name: "PunctualityComment",
                table: "StudentForms",
                newName: "AttendanceAndPunctualityComment");

            migrationBuilder.AddColumn<int>(
                name: "AverageReportScore",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageReportScore",
                table: "StudentForms");

            migrationBuilder.RenameColumn(
                name: "AttendanceAndPunctualityComment",
                table: "StudentForms",
                newName: "PunctualityComment");

            migrationBuilder.AddColumn<string>(
                name: "AttendanceComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
