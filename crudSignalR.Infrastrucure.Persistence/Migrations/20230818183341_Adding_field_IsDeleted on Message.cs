using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crudSignalR.Infrastrucure.Persistence.Migrations
{
    public partial class Adding_field_IsDeletedonMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Message",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Message");
        }
    }
}
