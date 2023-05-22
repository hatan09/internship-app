using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipApp.Core.Migrations
{
    public partial class updaterelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternGroups_Departments_DepartmentId",
                table: "InternGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_InternGroups_Instructors_InstructorId",
                table: "InternGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_InternGroups_InternGroupId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "InternGroups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_InternGroups_Departments_DepartmentId",
                table: "InternGroups",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InternGroups_Instructors_InstructorId",
                table: "InternGroups",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_InternGroups_InternGroupId",
                table: "Students",
                column: "InternGroupId",
                principalTable: "InternGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternGroups_Departments_DepartmentId",
                table: "InternGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_InternGroups_Instructors_InstructorId",
                table: "InternGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_InternGroups_InternGroupId",
                table: "Students");

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentId",
                table: "InternGroups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InternGroups_Departments_DepartmentId",
                table: "InternGroups",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InternGroups_Instructors_InstructorId",
                table: "InternGroups",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_InternGroups_InternGroupId",
                table: "Students",
                column: "InternGroupId",
                principalTable: "InternGroups",
                principalColumn: "Id");
        }
    }
}
