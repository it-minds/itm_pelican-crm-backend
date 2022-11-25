using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class addedlongdatecolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EndDate",
                table: "Deals",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastContactDate",
                table: "Deals",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StartDate",
                table: "Deals",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "LastContactDate",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Deals");
        }
    }
}
