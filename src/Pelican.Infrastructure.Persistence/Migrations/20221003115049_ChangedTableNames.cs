using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
	public partial class ChangedTableNames : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ClientContactPersons_Clients_ClientId",
				table: "ClientContactPersons");

			migrationBuilder.DropForeignKey(
				name: "FK_ClientContactPersons_ContactPersons_ClientId",
				table: "ClientContactPersons");

			migrationBuilder.DropForeignKey(
				name: "FK_DealContactPersons_ContactPersons_ContactId",
				table: "DealContactPersons");

			migrationBuilder.DropForeignKey(
				name: "FK_DealContactPersons_Deals_DealId",
				table: "DealContactPersons");

			migrationBuilder.DropForeignKey(
				name: "FK_Location_Suppliers_SupplierId",
				table: "Location");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Location",
				table: "Location");

			migrationBuilder.DropPrimaryKey(
				name: "PK_DealContactPersons",
				table: "DealContactPersons");

			migrationBuilder.DropPrimaryKey(
				name: "PK_ContactPersons",
				table: "ContactPersons");

			migrationBuilder.DropPrimaryKey(
				name: "PK_ClientContactPersons",
				table: "ClientContactPersons");

			migrationBuilder.RenameTable(
				name: "Location",
				newName: "Locations");

			migrationBuilder.RenameTable(
				name: "DealContactPersons",
				newName: "DealContacs");

			migrationBuilder.RenameTable(
				name: "ContactPersons",
				newName: "Contacts");

			migrationBuilder.RenameTable(
				name: "ClientContactPersons",
				newName: "ClientContacts");

			migrationBuilder.RenameIndex(
				name: "IX_Location_SupplierId",
				table: "Locations",
				newName: "IX_Locations_SupplierId");

			migrationBuilder.RenameIndex(
				name: "IX_DealContactPersons_DealId",
				table: "DealContacs",
				newName: "IX_DealContacs_DealId");

			migrationBuilder.RenameIndex(
				name: "IX_DealContactPersons_ContactId",
				table: "DealContacs",
				newName: "IX_DealContacs_ContactId");

			migrationBuilder.RenameIndex(
				name: "IX_ClientContactPersons_ClientId",
				table: "ClientContacts",
				newName: "IX_ClientContacts_ClientId");

			migrationBuilder.AddColumn<long>(
				name: "CreatedAt",
				table: "Locations",
				type: "bigint",
				nullable: false,
				defaultValue: 0L);

			migrationBuilder.AddColumn<long>(
				name: "LastUpdatedAt",
				table: "Locations",
				type: "bigint",
				nullable: true);

			migrationBuilder.AddPrimaryKey(
				name: "PK_Locations",
				table: "Locations",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_DealContacs",
				table: "DealContacs",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Contacts",
				table: "Contacts",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_ClientContacts",
				table: "ClientContacts",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_ClientContacts_Clients_ClientId",
				table: "ClientContacts",
				column: "ClientId",
				principalTable: "Clients",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_ClientContacts_Contacts_ClientId",
				table: "ClientContacts",
				column: "ClientId",
				principalTable: "Contacts",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

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

			migrationBuilder.AddForeignKey(
				name: "FK_Locations_Suppliers_SupplierId",
				table: "Locations",
				column: "SupplierId",
				principalTable: "Suppliers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_ClientContacts_Clients_ClientId",
				table: "ClientContacts");

			migrationBuilder.DropForeignKey(
				name: "FK_ClientContacts_Contacts_ClientId",
				table: "ClientContacts");

			migrationBuilder.DropForeignKey(
				name: "FK_DealContacs_Contacts_ContactId",
				table: "DealContacs");

			migrationBuilder.DropForeignKey(
				name: "FK_DealContacs_Deals_DealId",
				table: "DealContacs");

			migrationBuilder.DropForeignKey(
				name: "FK_Locations_Suppliers_SupplierId",
				table: "Locations");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Locations",
				table: "Locations");

			migrationBuilder.DropPrimaryKey(
				name: "PK_DealContacs",
				table: "DealContacs");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Contacts",
				table: "Contacts");

			migrationBuilder.DropPrimaryKey(
				name: "PK_ClientContacts",
				table: "ClientContacts");

			migrationBuilder.DropColumn(
				name: "CreatedAt",
				table: "Locations");

			migrationBuilder.DropColumn(
				name: "LastUpdatedAt",
				table: "Locations");

			migrationBuilder.RenameTable(
				name: "Locations",
				newName: "Location");

			migrationBuilder.RenameTable(
				name: "DealContacs",
				newName: "DealContactPersons");

			migrationBuilder.RenameTable(
				name: "Contacts",
				newName: "ContactPersons");

			migrationBuilder.RenameTable(
				name: "ClientContacts",
				newName: "ClientContactPersons");

			migrationBuilder.RenameIndex(
				name: "IX_Locations_SupplierId",
				table: "Location",
				newName: "IX_Location_SupplierId");

			migrationBuilder.RenameIndex(
				name: "IX_DealContacs_DealId",
				table: "DealContactPersons",
				newName: "IX_DealContactPersons_DealId");

			migrationBuilder.RenameIndex(
				name: "IX_DealContacs_ContactId",
				table: "DealContactPersons",
				newName: "IX_DealContactPersons_ContactId");

			migrationBuilder.RenameIndex(
				name: "IX_ClientContacts_ClientId",
				table: "ClientContactPersons",
				newName: "IX_ClientContactPersons_ClientId");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Location",
				table: "Location",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_DealContactPersons",
				table: "DealContactPersons",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_ContactPersons",
				table: "ContactPersons",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_ClientContactPersons",
				table: "ClientContactPersons",
				column: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_ClientContactPersons_Clients_ClientId",
				table: "ClientContactPersons",
				column: "ClientId",
				principalTable: "Clients",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_ClientContactPersons_ContactPersons_ClientId",
				table: "ClientContactPersons",
				column: "ClientId",
				principalTable: "ContactPersons",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_DealContactPersons_ContactPersons_ContactId",
				table: "DealContactPersons",
				column: "ContactId",
				principalTable: "ContactPersons",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_DealContactPersons_Deals_DealId",
				table: "DealContactPersons",
				column: "DealId",
				principalTable: "Deals",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Location_Suppliers_SupplierId",
				table: "Location",
				column: "SupplierId",
				principalTable: "Suppliers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
