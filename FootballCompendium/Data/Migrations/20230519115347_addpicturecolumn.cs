using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballCompendium.Data.Migrations
{
    public partial class addpicturecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "picture",
                table: "Stadium",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "picture",
                table: "Stadium");
        }
    }
}
