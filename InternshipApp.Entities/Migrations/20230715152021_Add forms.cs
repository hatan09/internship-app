using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipApp.Core.Migrations
{
    public partial class Addforms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseForms");

            migrationBuilder.AddColumn<int>(
                name: "AbilityToLearn",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AbilityToLearnComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AcademicPreparationOfTheStudents",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AcademicPreparationOfTheStudentsComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AcceptanceOfSuggestionsAndCriticisms",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AcceptanceOfSuggestionsAndCriticismsComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AnalyticalProblemSolvingSkills",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AnalyticalProblemSolvingSkillsComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Attendance",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AttendanceComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Dependability",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DependabilityComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "StudentForms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EvaluatedBy",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FulfilmentOfInternshipObjectives",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FulfilmentOfInternshipObjectivesComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GeneralInformation",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Independence",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IndependenceComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Initiative",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InitiativeComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "InterestInWorkResearch",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InterestInWorkResearchComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "StudentForms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationPlanning",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrganizationPlanningComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OverallPerformance",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OverallPerformanceComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OverallScore",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Punctuality",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PunctualityComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QualityOfWorkResearch",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QualityOfWorkResearchComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RelationsWithOthers",
                table: "StudentForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RelationsWithOthersComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "StudentForms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StudentComment",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "StudentForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SignalRConnectionId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LabourMarketForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TheEducationalGoalsAndProgramOutcomesAreClearAndAppropriateAsStated = table.Column<int>(type: "int", nullable: false),
                    TheCurriculumSatisfactorilyCoversTheProfessionOfComputingInTermsOfBreadthAndDepthOfKnowledge = table.Column<int>(type: "int", nullable: false),
                    AnAbilityToApplyProfessionalKnowledgeInPractice = table.Column<int>(type: "int", nullable: false),
                    AnAbilityToPlanOrganizeAndManageAProject = table.Column<int>(type: "int", nullable: false),
                    AnAbilityInProblemSolving = table.Column<int>(type: "int", nullable: false),
                    InitiativeTaking = table.Column<int>(type: "int", nullable: false),
                    AnAbilityToFunctionEffectivelyOnTeams = table.Column<int>(type: "int", nullable: false),
                    AnAbilityToCommunicateEffectively = table.Column<int>(type: "int", nullable: false),
                    AnAbilityToEngageInContinuingProfessionalDevelopment = table.Column<int>(type: "int", nullable: false),
                    Independence = table.Column<int>(type: "int", nullable: false),
                    RelationsWithOthers = table.Column<int>(type: "int", nullable: false),
                    IUGraduates = table.Column<int>(type: "int", nullable: false),
                    Prepared = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Improvements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabourMarketForms", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabourMarketForms");

            migrationBuilder.DropColumn(
                name: "AbilityToLearn",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "AbilityToLearnComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "AcademicPreparationOfTheStudents",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "AcademicPreparationOfTheStudentsComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "AcceptanceOfSuggestionsAndCriticisms",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "AcceptanceOfSuggestionsAndCriticismsComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "AnalyticalProblemSolvingSkills",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "AnalyticalProblemSolvingSkillsComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "Attendance",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "AttendanceComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "Dependability",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "DependabilityComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "EvaluatedBy",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "FulfilmentOfInternshipObjectives",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "FulfilmentOfInternshipObjectivesComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "GeneralInformation",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "Independence",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "IndependenceComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "Initiative",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "InitiativeComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "InterestInWorkResearch",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "InterestInWorkResearchComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "OrganizationPlanning",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "OrganizationPlanningComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "OverallPerformance",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "OverallPerformanceComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "OverallScore",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "Punctuality",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "PunctualityComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "QualityOfWorkResearch",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "QualityOfWorkResearchComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "RelationsWithOthers",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "RelationsWithOthersComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "StudentComment",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "StudentForms");

            migrationBuilder.DropColumn(
                name: "SignalRConnectionId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "CourseForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseForms", x => x.Id);
                });
        }
    }
}
