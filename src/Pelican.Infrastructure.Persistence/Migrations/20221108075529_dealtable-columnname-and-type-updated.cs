using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class dealtablecolumnnameandtypeupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HubspotContactId",
                table: "ClientContacts",
                newName: "HubSpotContactId");

            migrationBuilder.AlterColumn<string>(
                name: "HubSpotOwnerId",
                table: "Contacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HubSpotContactId",
                table: "ClientContacts",
                newName: "HubspotContactId");

            migrationBuilder.AlterColumn<string>(
                name: "HubSpotOwnerId",
                table: "Contacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
