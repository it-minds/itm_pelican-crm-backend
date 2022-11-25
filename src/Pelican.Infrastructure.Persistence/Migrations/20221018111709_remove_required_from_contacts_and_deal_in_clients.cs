using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
	public partial class remove_required_from_contacts_and_deal_in_clients : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Deals_Clients_ClientId",
				table: "Deals");

			migrationBuilder.AlterColumn<Guid>(
				name: "ClientId",
				table: "Deals",
				type: "uniqueidentifier",
				nullable: true,
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier");

			migrationBuilder.AddForeignKey(
				name: "FK_Deals_Clients_ClientId",
				table: "Deals",
				column: "ClientId",
				principalTable: "Clients",
				principalColumn: "Id");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Deals_Clients_ClientId",
				table: "Deals");

			migrationBuilder.AlterColumn<Guid>(
				name: "ClientId",
				table: "Deals",
				type: "uniqueidentifier",
				nullable: false,
				defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
				oldClrType: typeof(Guid),
				oldType: "uniqueidentifier",
				oldNullable: true);

			migrationBuilder.AddForeignKey(
				name: "FK_Deals_Clients_ClientId",
				table: "Deals",
				column: "ClientId",
				principalTable: "Clients",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
