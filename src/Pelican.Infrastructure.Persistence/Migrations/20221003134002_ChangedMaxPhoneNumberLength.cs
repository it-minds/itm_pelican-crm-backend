using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
	public partial class ChangedMaxPhoneNumberLength : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_DealContacs_Contacts_ContactId",
				table: "DealContacs");

			migrationBuilder.DropForeignKey(
				name: "FK_DealContacs_Deals_DealId",
				table: "DealContacs");

			migrationBuilder.DropPrimaryKey(
				name: "PK_DealContacs",
				table: "DealContacs");

			migrationBuilder.RenameTable(
				name: "DealContacs",
				newName: "DealContacts");

			migrationBuilder.RenameIndex(
				name: "IX_DealContacs_DealId",
				table: "DealContacts",
				newName: "IX_DealContacts_DealId");

			migrationBuilder.RenameIndex(
				name: "IX_DealContacs_ContactId",
				table: "DealContacts",
				newName: "IX_DealContacts_ContactId");

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "Suppliers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(15)",
				oldMaxLength: 15,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "Contacts",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(15)",
				oldMaxLength: 15,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "AccountManagers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(15)",
				oldMaxLength: 15,
				oldNullable: true);

			migrationBuilder.AddPrimaryKey(
				name: "PK_DealContacts",
				table: "DealContacts",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_DealContacts_Contacts_ContactId",
				table: "DealContacts",
				column: "ContactId",
				principalTable: "Contacts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_DealContacts_Deals_DealId",
				table: "DealContacts",
				column: "DealId",
				principalTable: "Deals",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_DealContacts_Contacts_ContactId",
				table: "DealContacts");

			migrationBuilder.DropForeignKey(
				name: "FK_DealContacts_Deals_DealId",
				table: "DealContacts");

			migrationBuilder.DropPrimaryKey(
				name: "PK_DealContacts",
				table: "DealContacts");

			migrationBuilder.RenameTable(
				name: "DealContacts",
				newName: "DealContacs");

			migrationBuilder.RenameIndex(
				name: "IX_DealContacts_DealId",
				table: "DealContacs",
				newName: "IX_DealContacs_DealId");

			migrationBuilder.RenameIndex(
				name: "IX_DealContacts_ContactId",
				table: "DealContacs",
				newName: "IX_DealContacs_ContactId");

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "Suppliers",
				type: "nvarchar(15)",
				maxLength: 15,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "Contacts",
				type: "nvarchar(15)",
				maxLength: 15,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PhoneNumber",
				table: "AccountManagers",
				type: "nvarchar(15)",
				maxLength: 15,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.AddPrimaryKey(
				name: "PK_DealContacs",
				table: "DealContacs",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_DealContacs_Contacts_ContactId",
				table: "DealContacs",
				column: "ContactId",
				principalTable: "Contacts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_DealContacs_Deals_DealId",
				table: "DealContacs",
				column: "DealId",
				principalTable: "Deals",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
