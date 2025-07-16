using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HostelManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixRoomForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allocations_AspNetUsers_StudentID",
                table: "Allocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Allocations_Rooms_RoomID",
                table: "Allocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Rooms_RoomId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_HostelBlocks_HostelBlockBlockID",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Applications_RoomId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Allocations_StudentID",
                table: "Allocations");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Allocations");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "Allocations");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "Allocations");

            migrationBuilder.RenameColumn(
                name: "HostelBlockBlockID",
                table: "Rooms",
                newName: "HostelBlockBlockId");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_HostelBlockBlockID",
                table: "Rooms",
                newName: "IX_Rooms_HostelBlockBlockId");

            migrationBuilder.RenameColumn(
                name: "BlockID",
                table: "HostelBlocks",
                newName: "BlockId");

            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "Allocations",
                newName: "RoomId");

            migrationBuilder.RenameColumn(
                name: "AllocationID",
                table: "Allocations",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Allocations_RoomID",
                table: "Allocations",
                newName: "IX_Allocations_RoomId");

            migrationBuilder.AlterColumn<int>(
                name: "HostelBlockBlockId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Allocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_ApplicationId",
                table: "Allocations",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Allocations_Applications_ApplicationId",
                table: "Allocations",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Allocations_Rooms_RoomId",
                table: "Allocations",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_HostelBlocks_HostelBlockBlockId",
                table: "Rooms",
                column: "HostelBlockBlockId",
                principalTable: "HostelBlocks",
                principalColumn: "BlockId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allocations_Applications_ApplicationId",
                table: "Allocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Allocations_Rooms_RoomId",
                table: "Allocations");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_HostelBlocks_HostelBlockBlockId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Allocations_ApplicationId",
                table: "Allocations");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Allocations");

            migrationBuilder.RenameColumn(
                name: "HostelBlockBlockId",
                table: "Rooms",
                newName: "HostelBlockBlockID");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_HostelBlockBlockId",
                table: "Rooms",
                newName: "IX_Rooms_HostelBlockBlockID");

            migrationBuilder.RenameColumn(
                name: "BlockId",
                table: "HostelBlocks",
                newName: "BlockID");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Allocations",
                newName: "RoomID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Allocations",
                newName: "AllocationID");

            migrationBuilder.RenameIndex(
                name: "IX_Allocations_RoomId",
                table: "Allocations",
                newName: "IX_Allocations_RoomID");

            migrationBuilder.AlterColumn<int>(
                name: "HostelBlockBlockID",
                table: "Rooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "Allocations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StudentID",
                table: "Allocations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "Allocations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_RoomId",
                table: "Applications",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_StudentID",
                table: "Allocations",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Allocations_AspNetUsers_StudentID",
                table: "Allocations",
                column: "StudentID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Allocations_Rooms_RoomID",
                table: "Allocations",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Rooms_RoomId",
                table: "Applications",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_HostelBlocks_HostelBlockBlockID",
                table: "Rooms",
                column: "HostelBlockBlockID",
                principalTable: "HostelBlocks",
                principalColumn: "BlockID");
        }
    }
}
