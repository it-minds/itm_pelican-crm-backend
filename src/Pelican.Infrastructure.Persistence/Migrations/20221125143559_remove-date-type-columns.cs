using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class removedatetypecolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Deals",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastContactDate",
                table: "Deals",
                type: "Date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Deals",
                type: "Date",
                nullable: true);
        }
    }
}
