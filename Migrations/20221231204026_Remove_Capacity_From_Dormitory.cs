using Microsoft.EntityFrameworkCore.Migrations;

namespace DbLabProject.Migrations
{
    public partial class Remove_Capacity_From_Dormitory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailablePlaces",
                table: "Dormitories");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Dormitories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailablePlaces",
                table: "Dormitories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Dormitories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
