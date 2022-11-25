using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class AddedWebsiteToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Clients_ClientId",
                table: "Deals");

            migrationBuilder.RenameColumn(
                name: "HubspotContactId",
                table: "ClientContacts",
                newName: "HubSpotContactId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "Deals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Clients",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Clients_ClientId",
                table: "Deals",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Clients_ClientId",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "HubSpotContactId",
                table: "ClientContacts",
                newName: "HubspotContactId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClientId",
                table: "Deals",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "HubSpotOwnerId",
                table: "Contacts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_Clients_ClientId",
                table: "Deals",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
