using Microsoft.EntityFrameworkCore.Migrations;

namespace Senparc.Web.Migrations
{
    public partial class UpdateTableRelateV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "ProjectMembers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScheduleId",
                table: "CompetitionPrograms",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionPrograms_ScheduleId",
                table: "CompetitionPrograms",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionPrograms_Schedules_ScheduleId",
                table: "CompetitionPrograms",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectMembers_CompetitionPrograms_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId",
                principalTable: "CompetitionPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionPrograms_Schedules_ScheduleId",
                table: "CompetitionPrograms");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectMembers_CompetitionPrograms_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_CompetitionPrograms_ScheduleId",
                table: "CompetitionPrograms");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "CompetitionPrograms");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "ProjectMembers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
