using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TierListes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Domain = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "DisplayOrder", "Domain", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 1, "nintendo.com", "Nintendo" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 2, "playstation.com", "PlayStation" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 3, "xbox.com", "Xbox" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 4, "ea.com", "EA" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 5, "ubisoft.com", "Ubisoft" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 6, "activisionblizzard.com", "Activision Blizzard" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), 7, "rockstargames.com", "Rockstar Games" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 8, "square-enix.com", "Square Enix" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), 9, "capcom.com", "Capcom" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 10, "epicgames.com", "Epic Games" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
