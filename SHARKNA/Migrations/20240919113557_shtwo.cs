using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHARKNA.Migrations
{
    public partial class shtwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEventAttendence_tblEvents_EventsId",
                table: "tblEventAttendence");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventsId",
                table: "tblEventAttendence",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "tblEventAttendenceLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    evattend = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEventAttendenceLogs", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_tblEventAttendence_tblEvents_EventsId",
                table: "tblEventAttendence",
                column: "EventsId",
                principalTable: "tblEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEventAttendence_tblEvents_EventsId",
                table: "tblEventAttendence");

            migrationBuilder.DropTable(
                name: "tblEventAttendenceLogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventsId",
                table: "tblEventAttendence",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEventAttendence_tblEvents_EventsId",
                table: "tblEventAttendence",
                column: "EventsId",
                principalTable: "tblEvents",
                principalColumn: "Id");
        }
    }
}
