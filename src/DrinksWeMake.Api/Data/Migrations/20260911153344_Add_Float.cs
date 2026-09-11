using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrinksWeMake.Api.Migrations
{
    /// <inheritdoc />
    public partial class Add_Float : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "float",
                table: "cocktail_ingredients",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "float",
                table: "cocktail_ingredients");
        }
    }
}
