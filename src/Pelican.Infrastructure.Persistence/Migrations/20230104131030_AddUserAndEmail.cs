using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pelican.Infrastructure.Persistence.Migrations
{
    public partial class AddUserAndEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Heading1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Paragraph1 = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: false),
                    Heading2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Paragraph2 = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: false),
                    Heading3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Paragraph3 = table.Column<string>(type: "nvarchar(1200)", maxLength: 1200, nullable: false),
                    CtaButtonText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    SSOTokenId = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdatedAt = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
