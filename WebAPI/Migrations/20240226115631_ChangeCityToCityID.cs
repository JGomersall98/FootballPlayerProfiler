using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatchMasterWEB.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCityToCityID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Fixtures");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Fixtures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Fixtures");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Fixtures",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
