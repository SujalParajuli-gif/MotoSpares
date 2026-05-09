using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoSpares.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MS_PART",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MS_PART");
        }
    }
}
