using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace crudSegnalR.Infrastructure.Identity.Migrations
{
    public partial class AddingpropertyIsOnlineinmodelforuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                schema: "Identity",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnline",
                schema: "Identity",
                table: "Users");
        }
    }
}
