﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHARKNA.Migrations
{
    public partial class DeleteT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEventRegistrations_tblEvents_EventsId",
                table: "tblEventRegistrations");

            migrationBuilder.DropColumn(
                name: "EventstId",
                table: "tblEventRegistrations");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventsId",
                table: "tblEventRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tblEventRegistrations_tblEvents_EventsId",
                table: "tblEventRegistrations",
                column: "EventsId",
                principalTable: "tblEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEventRegistrations_tblEvents_EventsId",
                table: "tblEventRegistrations");

            migrationBuilder.AlterColumn<Guid>(
                name: "EventsId",
                table: "tblEventRegistrations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EventstId",
                table: "tblEventRegistrations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_tblEventRegistrations_tblEvents_EventsId",
                table: "tblEventRegistrations",
                column: "EventsId",
                principalTable: "tblEvents",
                principalColumn: "Id");
        }
    }
}
