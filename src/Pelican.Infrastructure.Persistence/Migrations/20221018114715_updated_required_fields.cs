using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
	public partial class updated_required_fields : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<DateTime>(
				name: "EndDate",
				table: "Deals",
				type: "Date",
				nullable: true,
				oldClrType: typeof(DateTime),
				oldType: "Date");

			migrationBuilder.AlterColumn<string>(
				name: "HubSpotDealId",
				table: "DealContacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "HubSpotContactId",
				table: "DealContacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "Contacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "HubspotContactId",
				table: "ClientContacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "HubspotClientId",
				table: "ClientContacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "HubSpotDealId",
				table: "AccountManagerDeals",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "HubSpotAccountManagerId",
				table: "AccountManagerDeals",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<DateTime>(
				name: "EndDate",
				table: "Deals",
				type: "Date",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
				oldClrType: typeof(DateTime),
				oldType: "Date",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "HubSpotDealId",
				table: "DealContacts",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "HubSpotContactId",
				table: "DealContacts",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "Contacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "HubspotContactId",
				table: "ClientContacts",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "HubspotClientId",
				table: "ClientContacts",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "HubSpotDealId",
				table: "AccountManagerDeals",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			migrationBuilder.AlterColumn<string>(
				name: "HubSpotAccountManagerId",
				table: "AccountManagerDeals",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);
		}
	}
}
