using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalRChat.Migrations
{
    public partial class SendDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Message");

            migrationBuilder.AddColumn<DateTime>(
                name: "SendDate",
                table: "Message",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendDate",
                table: "Message");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Message",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
