using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class DealNameAndDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "Contacts",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "Contacts",
                newName: "FirstName");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Deals",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Deals",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Contacts",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Contacts",
                newName: "Firstname");
        }
    }
}
