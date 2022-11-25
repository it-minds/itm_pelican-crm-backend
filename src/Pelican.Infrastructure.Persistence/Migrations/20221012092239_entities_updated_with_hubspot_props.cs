using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
	public partial class entities_updated_with_hubspot_props : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ClientContacts_Contacts_ClientId",
				table: "ClientContacts");

			migrationBuilder.DropColumn(
				name: "Name",
				table: "Contacts");

			migrationBuilder.AlterColumn<string>(
				name: "WebsiteUrl",
				table: "Suppliers",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(1000)",
				oldMaxLength: 1000,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PictureUrl",
				table: "Suppliers",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(1000)",
				oldMaxLength: 1000,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "Suppliers",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "Suppliers",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "LinkedInUrl",
				table: "Suppliers",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(1000)",
				oldMaxLength: 1000,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "Suppliers",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.AddColumn<string>(
				name: "HubSpotDomain",
				table: "Suppliers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<long>(
				name: "HubSpotId",
				table: "Suppliers",
				type: "bigint",
				nullable: false,
				defaultValue: 0L);

			migrationBuilder.AddColumn<string>(
				name: "RefreshToken",
				table: "Suppliers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AlterColumn<string>(
				name: "CityName",
				table: "Locations",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(200)",
				oldMaxLength: 200);

			migrationBuilder.AlterColumn<decimal>(
				name: "Revenue",
				table: "Deals",
				type: "decimal(18,2)",
				nullable: true,
				oldClrType: typeof(decimal),
				oldType: "decimal(19,4)",
				oldNullable: true);

			migrationBuilder.AlterColumn<DateTime>(
				name: "EndDate",
				table: "Deals",
				type: "datetime2",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "Date");

			migrationBuilder.AlterColumn<string>(
				name: "DealStatus",
				table: "Deals",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(200)",
				oldMaxLength: 200);

			migrationBuilder.AddColumn<string>(
				name: "CurrencyCode",
				table: "Deals",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "HubSpotId",
				table: "Deals",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "Contacts",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "LinkedInUrl",
				table: "Contacts",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(1000)",
				oldMaxLength: 1000,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "Contacts",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AddColumn<string>(
				name: "Firstname",
				table: "Contacts",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "Lastname",
				table: "Contacts",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AlterColumn<string>(
				name: "Segment",
				table: "Clients",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(200)",
				oldMaxLength: 200,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PictureUrl",
				table: "Clients",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(1000)",
				oldMaxLength: 1000,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "OfficeLocation",
				table: "Clients",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(200)",
				oldMaxLength: 200,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "Clients",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "Classification",
				table: "Clients",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(200)",
				oldMaxLength: 200,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PictureUrl",
				table: "AccountManagers",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(1000)",
				oldMaxLength: 1000,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "AccountManagers",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "AccountManagers",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "LinkedInUrl",
				table: "AccountManagers",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(1000)",
				oldMaxLength: 1000,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "AccountManagers",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AddColumn<string>(
				name: "HubSpotId",
				table: "AccountManagers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<long>(
				name: "HubSpotUserId",
				table: "AccountManagers",
				type: "bigint",
				nullable: false,
				defaultValue: 0L);

			migrationBuilder.CreateIndex(
				name: "IX_ClientContacts_ContactId",
				table: "ClientContacts",
				column: "ContactId");

			migrationBuilder.AddForeignKey(
				name: "FK_ClientContacts_Contacts_ContactId",
				table: "ClientContacts",
				column: "ContactId",
				principalTable: "Contacts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ClientContacts_Contacts_ContactId",
				table: "ClientContacts");

			migrationBuilder.DropIndex(
				name: "IX_ClientContacts_ContactId",
				table: "ClientContacts");

			migrationBuilder.DropColumn(
				name: "HubSpotDomain",
				table: "Suppliers");

			migrationBuilder.DropColumn(
				name: "HubSpotId",
				table: "Suppliers");

			migrationBuilder.DropColumn(
				name: "RefreshToken",
				table: "Suppliers");

			migrationBuilder.DropColumn(
				name: "CurrencyCode",
				table: "Deals");

			migrationBuilder.DropColumn(
				name: "HubSpotId",
				table: "Deals");

			migrationBuilder.DropColumn(
				name: "Firstname",
				table: "Contacts");

			migrationBuilder.DropColumn(
				name: "Lastname",
				table: "Contacts");

			migrationBuilder.DropColumn(
				name: "HubSpotId",
				table: "AccountManagers");

			migrationBuilder.DropColumn(
				name: "HubSpotUserId",
				table: "AccountManagers");

			migrationBuilder.AlterColumn<string>(
				name: "WebsiteUrl",
				table: "Suppliers",
				type: "nvarchar(1000)",
				maxLength: 1000,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PictureUrl",
				table: "Suppliers",
				type: "nvarchar(1000)",
				maxLength: 1000,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "Suppliers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "Suppliers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "LinkedInUrl",
				table: "Suppliers",
				type: "nvarchar(1000)",
				maxLength: 1000,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "Suppliers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "CityName",
				table: "Locations",
				type: "nvarchar(200)",
				maxLength: 200,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<decimal>(
				name: "Revenue",
				table: "Deals",
				type: "decimal(19,4)",
				nullable: true,
				oldClrType: typeof(decimal),
				oldType: "decimal(18,2)",
				oldNullable: true);

			migrationBuilder.AlterColumn<DateTime>(
				name: "EndDate",
				table: "Deals",
				type: "Date",
				nullable: false,
				oldClrType: typeof(DateTime),
				oldType: "datetime2");

			migrationBuilder.AlterColumn<string>(
				name: "DealStatus",
				table: "Deals",
				type: "nvarchar(200)",
				maxLength: 200,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "Contacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "LinkedInUrl",
				table: "Contacts",
				type: "nvarchar(1000)",
				maxLength: 1000,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "Contacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AddColumn<string>(
				name: "Name",
				table: "Contacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AlterColumn<string>(
				name: "Segment",
				table: "Clients",
				type: "nvarchar(200)",
				maxLength: 200,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PictureUrl",
				table: "Clients",
				type: "nvarchar(1000)",
				maxLength: 1000,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "OfficeLocation",
				table: "Clients",
				type: "nvarchar(200)",
				maxLength: 200,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "Clients",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "Classification",
				table: "Clients",
				type: "nvarchar(200)",
				maxLength: 200,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PictureUrl",
				table: "AccountManagers",
				type: "nvarchar(1000)",
				maxLength: 1000,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "AccountManagers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Name",
				table: "AccountManagers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "LinkedInUrl",
				table: "AccountManagers",
				type: "nvarchar(1000)",
				maxLength: 1000,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "AccountManagers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AddForeignKey(
				name: "FK_ClientContacts_Contacts_ClientId",
				table: "ClientContacts",
				column: "ClientId",
				principalTable: "Contacts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
