using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class SourceUpdateTimeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SourceUpdateTimestamp",
                table: "Deals",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceUpdateTimestamp",
                table: "Contacts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceUpdateTimestamp",
                table: "Clients",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SourceUpdateTimestamp",
                table: "AccountManagers",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceUpdateTimestamp",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "SourceUpdateTimestamp",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "SourceUpdateTimestamp",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "SourceUpdateTimestamp",
                table: "AccountManagers");
        }
    }
}
