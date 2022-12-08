using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class SourceUpdateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SourceUpdateTimestamp",
                table: "Deals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SourceUpdateTimestamp",
                table: "Contacts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SourceUpdateTimestamp",
                table: "Clients",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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
