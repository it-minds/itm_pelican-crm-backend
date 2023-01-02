using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
	public partial class TypeStartDateOnDealEntity : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<DateTime>(
				name: "StartDate",
				table: "Deals",
				type: "Date",
				nullable: true,
				oldClrType: typeof(DateTime),
				oldType: "datetime2",
				oldNullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<DateTime>(
				name: "StartDate",
				table: "Deals",
				type: "datetime2",
				nullable: true,
				oldClrType: typeof(DateTime),
				oldType: "Date",
				oldNullable: true);
		}
	}
}
