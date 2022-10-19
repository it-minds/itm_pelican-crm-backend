using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class clientcontact_relation_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientContacts_Contacts_ClientId",
                table: "ClientContacts");

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
