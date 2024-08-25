using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHARKNA.Migrations
{
    public partial class Sharkna : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventRegistrationsViewModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectionReasons = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullNameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tblRequestStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrationsViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistrationsViewModel_tblRequestStatus_tblRequestStatusId",
                        column: x => x.tblRequestStatusId,
                        principalTable: "tblRequestStatus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrationsViewModel_tblRequestStatusId",
                table: "EventRegistrationsViewModel",
                column: "tblRequestStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventRegistrationsViewModel");
        }
    }
}
