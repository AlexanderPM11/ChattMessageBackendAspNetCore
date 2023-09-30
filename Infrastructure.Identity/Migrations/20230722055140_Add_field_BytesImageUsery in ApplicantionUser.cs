using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crudSegnalR.Infrastructure.Identity.Migrations
{
    public partial class Add_field_BytesImageUseryinApplicantionUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "BytesImageUsery",
                schema: "Identity",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BytesImageUsery",
                schema: "Identity",
                table: "Users");
        }
    }
}
