using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballCompendium.Data.Migrations
{
    public partial class description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "picture",
                table: "Stadium",
                newName: "description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "description",
                table: "Stadium",
                newName: "picture");
        }
    }
}
