using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TierListes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionToTierList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "TierLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "TierLists");
        }
    }
}
