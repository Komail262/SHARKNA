using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SHARKNA.Migrations
{
    public partial class adddb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblBoardMembersLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoMeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBoardMembersLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblBoardRequestLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReqId = table.Column<int>(type: "int", nullable: false),
                    OpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBoardRequestLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblBoardRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBoardRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblBoards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBoards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblBoardTalRequestLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReqId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBoardTalRequestLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblEventLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEventLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblEventMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEventMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblEventRegLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEventRegLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblEventRequestLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReqId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEventRequestLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventTitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventEndtDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndRegTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpeakersAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpeakersEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopicEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxAttendence = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblPermmisionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPermmisionLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblRequestStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestStatusAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestStatusEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRequestStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    FullNameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullNameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblBoardMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BoardRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBoardMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBoardMembers_tblBoardRoles_BoardRoleId",
                        column: x => x.BoardRoleId,
                        principalTable: "tblBoardRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBoardMembers_tblBoards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "tblBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblEventAttendence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    EventsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventstId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventMemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAttend = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEventAttendence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEventAttendence_tblEventMembers_EventMemberId",
                        column: x => x.EventMemberId,
                        principalTable: "tblEventMembers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblEventAttendence_tblEvents_EventsId",
                        column: x => x.EventsId,
                        principalTable: "tblEvents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tblBoardTalRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Experiences = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReasons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBoardTalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBoardTalRequests_tblBoards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "tblBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBoardTalRequests_tblRequestStatus_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "tblRequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblEventRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectionReasons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEventRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEventRegistrations_tblEvents_EventsId",
                        column: x => x.EventsId,
                        principalTable: "tblEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEventRegistrations_tblRequestStatus_EventStatusId",
                        column: x => x.EventStatusId,
                        principalTable: "tblRequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblEventRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RejectionReasons = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEventRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEventRequests_tblBoards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "tblBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEventRequests_tblEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "tblEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEventRequests_tblRequestStatus_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "tblRequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblPermissions_tblRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tblRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblBoardRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardRolesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BoardRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardMemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RejectionReasons = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBoardRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblBoardRequests_tblBoardMembers_BoardMemberId",
                        column: x => x.BoardMemberId,
                        principalTable: "tblBoardMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBoardRequests_tblBoardRoles_BoardRolesId",
                        column: x => x.BoardRolesId,
                        principalTable: "tblBoardRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblBoardRequests_tblBoards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "tblBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBoardRequests_tblRequestStatus_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "tblRequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblBoardMembers_BoardId",
                table: "tblBoardMembers",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBoardMembers_BoardRoleId",
                table: "tblBoardMembers",
                column: "BoardRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBoardRequests_BoardId",
                table: "tblBoardRequests",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBoardRequests_BoardMemberId",
                table: "tblBoardRequests",
                column: "BoardMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBoardRequests_BoardRolesId",
                table: "tblBoardRequests",
                column: "BoardRolesId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBoardRequests_RequestStatusId",
                table: "tblBoardRequests",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBoardTalRequests_BoardId",
                table: "tblBoardTalRequests",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_tblBoardTalRequests_RequestStatusId",
                table: "tblBoardTalRequests",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventAttendence_EventMemberId",
                table: "tblEventAttendence",
                column: "EventMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventAttendence_EventsId",
                table: "tblEventAttendence",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventRegistrations_EventsId",
                table: "tblEventRegistrations",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventRegistrations_EventStatusId",
                table: "tblEventRegistrations",
                column: "EventStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventRequests_BoardId",
                table: "tblEventRequests",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventRequests_EventId",
                table: "tblEventRequests",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventRequests_RequestStatusId",
                table: "tblEventRequests",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tblPermissions_RoleId",
                table: "tblPermissions",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblBoardMembersLogs");

            migrationBuilder.DropTable(
                name: "tblBoardRequestLogs");

            migrationBuilder.DropTable(
                name: "tblBoardRequests");

            migrationBuilder.DropTable(
                name: "tblBoardTalRequestLogs");

            migrationBuilder.DropTable(
                name: "tblBoardTalRequests");

            migrationBuilder.DropTable(
                name: "tblEventAttendence");

            migrationBuilder.DropTable(
                name: "tblEventLogs");

            migrationBuilder.DropTable(
                name: "tblEventRegistrations");

            migrationBuilder.DropTable(
                name: "tblEventRegLogs");

            migrationBuilder.DropTable(
                name: "tblEventRequestLogs");

            migrationBuilder.DropTable(
                name: "tblEventRequests");

            migrationBuilder.DropTable(
                name: "tblPermissions");

            migrationBuilder.DropTable(
                name: "tblPermmisionLogs");

            migrationBuilder.DropTable(
                name: "tblUsers");

            migrationBuilder.DropTable(
                name: "tblBoardMembers");

            migrationBuilder.DropTable(
                name: "tblEventMembers");

            migrationBuilder.DropTable(
                name: "tblEvents");

            migrationBuilder.DropTable(
                name: "tblRequestStatus");

            migrationBuilder.DropTable(
                name: "tblRoles");

            migrationBuilder.DropTable(
                name: "tblBoardRoles");

            migrationBuilder.DropTable(
                name: "tblBoards");
        }
    }
}
