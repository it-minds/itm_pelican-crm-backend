using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class SourceAndChangedHubSpotIdToSOurceIdToEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HubSpotId",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "HubSpotOwnerId",
                table: "Deals",
                newName: "SourceOwnerId");

            migrationBuilder.RenameColumn(
                name: "HubSpotId",
                table: "Deals",
                newName: "SourceId");

            migrationBuilder.RenameColumn(
                name: "HubSpotDealId",
                table: "DealContacts",
                newName: "SourceDealId");

            migrationBuilder.RenameColumn(
                name: "HubSpotContactId",
                table: "DealContacts",
                newName: "SourceContactId");

            migrationBuilder.RenameColumn(
                name: "HubSpotOwnerId",
                table: "Contacts",
                newName: "SourceOwnerId");

            migrationBuilder.RenameColumn(
                name: "HubSpotId",
                table: "Contacts",
                newName: "SourceId");

            migrationBuilder.RenameColumn(
                name: "HubSpotId",
                table: "Clients",
                newName: "SourceId");

            migrationBuilder.RenameColumn(
                name: "HubSpotContactId",
                table: "ClientContacts",
                newName: "SourceContactId");

            migrationBuilder.RenameColumn(
                name: "HubSpotClientId",
                table: "ClientContacts",
                newName: "SourceClientId");

            migrationBuilder.RenameColumn(
                name: "HubSpotUserId",
                table: "AccountManagers",
                newName: "SourceUserId");

            migrationBuilder.RenameColumn(
                name: "HubSpotId",
                table: "AccountManagers",
                newName: "SourceId");

            migrationBuilder.RenameColumn(
                name: "HubSpotDealId",
                table: "AccountManagerDeals",
                newName: "SourceDealId");

            migrationBuilder.RenameColumn(
                name: "HubSpotAccountManagerId",
                table: "AccountManagerDeals",
                newName: "SourceAccountManagerId");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Suppliers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SourceId",
                table: "Suppliers",
                type: "bigint",
                maxLength: 100,
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Deals",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Contacts",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Clients",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "AccountManagers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "SourceId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "AccountManagers");

            migrationBuilder.RenameColumn(
                name: "SourceOwnerId",
                table: "Deals",
                newName: "HubSpotOwnerId");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "Deals",
                newName: "HubSpotId");

            migrationBuilder.RenameColumn(
                name: "SourceDealId",
                table: "DealContacts",
                newName: "HubSpotDealId");

            migrationBuilder.RenameColumn(
                name: "SourceContactId",
                table: "DealContacts",
                newName: "HubSpotContactId");

            migrationBuilder.RenameColumn(
                name: "SourceOwnerId",
                table: "Contacts",
                newName: "HubSpotOwnerId");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "Contacts",
                newName: "HubSpotId");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "Clients",
                newName: "HubSpotId");

            migrationBuilder.RenameColumn(
                name: "SourceContactId",
                table: "ClientContacts",
                newName: "HubSpotContactId");

            migrationBuilder.RenameColumn(
                name: "SourceClientId",
                table: "ClientContacts",
                newName: "HubSpotClientId");

            migrationBuilder.RenameColumn(
                name: "SourceUserId",
                table: "AccountManagers",
                newName: "HubSpotUserId");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "AccountManagers",
                newName: "HubSpotId");

            migrationBuilder.RenameColumn(
                name: "SourceDealId",
                table: "AccountManagerDeals",
                newName: "HubSpotDealId");

            migrationBuilder.RenameColumn(
                name: "SourceAccountManagerId",
                table: "AccountManagerDeals",
                newName: "HubSpotAccountManagerId");

            migrationBuilder.AddColumn<long>(
                name: "HubSpotId",
                table: "Suppliers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
