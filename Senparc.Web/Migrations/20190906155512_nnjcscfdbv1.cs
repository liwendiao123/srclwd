using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Senparc.Web.Migrations
{
    public partial class nnjcscfdbv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Flag = table.Column<bool>(nullable: false),
                    CoverUrl = table.Column<string>(unicode: false, nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IsPublish = table.Column<bool>(nullable: false),
                    IssueTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetitionPrograms",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Flag = table.Column<bool>(nullable: false),
                    Cate = table.Column<int>(nullable: false),
                    ImgUrl = table.Column<string>(unicode: false, nullable: true),
                    BdImgUrl = table.Column<string>(unicode: false, nullable: true),
                    BdImgUrlPwd = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    ControlId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Company = table.Column<string>(nullable: true),
                    CreatorId = table.Column<string>(nullable: true),
                    CreatorName = table.Column<string>(nullable: true),
                    UpdatorId = table.Column<string>(nullable: true),
                    UpdatorName = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Flag = table.Column<bool>(nullable: false),
                    ProjectId = table.Column<string>(nullable: true),
                    IdCard = table.Column<string>(nullable: true),
                    HeadImgUrl = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IdCardImgUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Nation = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    IsLeader = table.Column<bool>(nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Flag = table.Column<bool>(nullable: false),
                    ActivityId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    SignNumber = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    CreatorId = table.Column<string>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    UpdatorId = table.Column<string>(nullable: true),
                    UpdatorName = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_ActivityId",
                table: "Schedules",
                column: "ActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionPrograms");

            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Activities");
        }
    }
}
