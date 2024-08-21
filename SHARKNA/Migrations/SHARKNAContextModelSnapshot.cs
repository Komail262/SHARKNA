﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SHARKNA.Models;

#nullable disable

namespace SHARKNA.Migrations
{
    [DbContext(typeof(SHARKNAContext))]
    partial class SHARKNAContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SHARKNA.Models.tblBoardMembers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoardRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.HasIndex("BoardRoleId");

                    b.ToTable("tblBoardMembers");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardMembersLogs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BoMeId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OpDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OpType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblBoardMembersLogs");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardRequestLogs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OpDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OpType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReqId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("tblBoardRequestLogs");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardRequests", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RejectionReasons")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RequestStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.HasIndex("RequestStatusId");

                    b.ToTable("tblBoardRequests");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardRoles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("NameAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblBoardRoles");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoards", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DescriptionAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("NameAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblBoards");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardTalRequestLogs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OpDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OpType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReqId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("tblBoardTalRequestLogs");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardTalRequests", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Experiences")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RejectionReasons")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RequestStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Skills")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.HasIndex("RequestStatusId");

                    b.ToTable("tblBoardTalRequests");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventAttendence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("EventsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventsRegId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventstId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsAttend")
                        .HasColumnType("bit");

                    b.Property<Guid?>("tblEventMembersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("EventsId");

                    b.HasIndex("EventsRegId");

                    b.HasIndex("tblEventMembersId");

                    b.ToTable("tblEventAttendence");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventLogs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EvId")
                        .HasColumnType("int");

                    b.Property<DateTime>("OpDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OpType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblEventLogs");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventMembers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullNameAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblEventMembers");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventRegistrations", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EventStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullNameAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RejectionReasons")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EventStatusId");

                    b.HasIndex("EventsId");

                    b.ToTable("tblEventRegistrations");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventRegLogs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EvId")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OpDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OpType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblEventRegLogs");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventRequestLogs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OpDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("OpType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReqId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("tblEventRequestLogs");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventRequests", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RejectionReasons")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RequestStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.HasIndex("EventId");

                    b.HasIndex("RequestStatusId");

                    b.ToTable("tblEventRequests");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEvents", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DescriptionAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndRegTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventEndtDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EventStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventTitleAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventTitleEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LocationAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocationEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxAttendence")
                        .HasColumnType("int");

                    b.Property<string>("SpeakersAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpeakersEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time");

                    b.Property<string>("TopicAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TopicEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblEvents");
                });

            modelBuilder.Entity("SHARKNA.Models.tblPermissions", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullNameAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullNameEn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("tblPermissions");
                });

            modelBuilder.Entity("SHARKNA.Models.tblPermmisionLogs", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OpType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PermId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("tblPermmisionLogs");
                });

            modelBuilder.Entity("SHARKNA.Models.tblRequestStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("RequestStatusAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestStatusEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblRequestStatus");
                });

            modelBuilder.Entity("SHARKNA.Models.tblRoles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NameAr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameEn")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("tblRoles");
                });

            modelBuilder.Entity("SHARKNA.Models.tblUsers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullNameAr")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullNameEn")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("tblUsers");
                });

            modelBuilder.Entity("SHARKNA.ViewModels.EventRegistrationsViewModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullNameAr")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullNameEn")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RejectionReasons")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("RequestStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("tblRequestStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("tblRequestStatusId");

                    b.ToTable("EventRegistrationsViewModel");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardMembers", b =>
                {
                    b.HasOne("SHARKNA.Models.tblBoards", "Board")
                        .WithMany("BoardMembers")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SHARKNA.Models.tblBoardRoles", "BoardRole")
                        .WithMany("BoardMember")
                        .HasForeignKey("BoardRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("BoardRole");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardRequests", b =>
                {
                    b.HasOne("SHARKNA.Models.tblBoards", "Board")
                        .WithMany("BoardRequests")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SHARKNA.Models.tblRequestStatus", "RequestStatus")
                        .WithMany("BoardReq")
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("RequestStatus");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardTalRequests", b =>
                {
                    b.HasOne("SHARKNA.Models.tblBoards", "Board")
                        .WithMany("BoardTalRequests")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SHARKNA.Models.tblRequestStatus", "RequestStatus")
                        .WithMany("BoardTalReq")
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("RequestStatus");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventAttendence", b =>
                {
                    b.HasOne("SHARKNA.Models.tblEvents", "Events")
                        .WithMany()
                        .HasForeignKey("EventsId");

                    b.HasOne("SHARKNA.Models.tblEventRegistrations", "EventsReg")
                        .WithMany()
                        .HasForeignKey("EventsRegId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SHARKNA.Models.tblEventMembers", null)
                        .WithMany("EventAttend")
                        .HasForeignKey("tblEventMembersId");

                    b.Navigation("Events");

                    b.Navigation("EventsReg");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventRegistrations", b =>
                {
                    b.HasOne("SHARKNA.Models.tblRequestStatus", "EventStatus")
                        .WithMany()
                        .HasForeignKey("EventStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SHARKNA.Models.tblEvents", "Events")
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventStatus");

                    b.Navigation("Events");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventRequests", b =>
                {
                    b.HasOne("SHARKNA.Models.tblBoards", "Board")
                        .WithMany("EventRequests")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SHARKNA.Models.tblEvents", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SHARKNA.Models.tblRequestStatus", "RequestStatus")
                        .WithMany("EventReq")
                        .HasForeignKey("RequestStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("Event");

                    b.Navigation("RequestStatus");
                });

            modelBuilder.Entity("SHARKNA.Models.tblPermissions", b =>
                {
                    b.HasOne("SHARKNA.Models.tblRoles", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SHARKNA.ViewModels.EventRegistrationsViewModel", b =>
                {
                    b.HasOne("SHARKNA.Models.tblRequestStatus", null)
                        .WithMany("EventReg")
                        .HasForeignKey("tblRequestStatusId");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoardRoles", b =>
                {
                    b.Navigation("BoardMember");
                });

            modelBuilder.Entity("SHARKNA.Models.tblBoards", b =>
                {
                    b.Navigation("BoardMembers");

                    b.Navigation("BoardRequests");

                    b.Navigation("BoardTalRequests");

                    b.Navigation("EventRequests");
                });

            modelBuilder.Entity("SHARKNA.Models.tblEventMembers", b =>
                {
                    b.Navigation("EventAttend");
                });

            modelBuilder.Entity("SHARKNA.Models.tblRequestStatus", b =>
                {
                    b.Navigation("BoardReq");

                    b.Navigation("BoardTalReq");

                    b.Navigation("EventReg");

                    b.Navigation("EventReq");
                });

            modelBuilder.Entity("SHARKNA.Models.tblRoles", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
