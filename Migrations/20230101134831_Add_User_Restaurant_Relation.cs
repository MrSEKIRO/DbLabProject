using Microsoft.EntityFrameworkCore.Migrations;

namespace DbLabProject.Migrations
{
    public partial class Add_User_Restaurant_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RestaurantStudent",
                columns: table => new
                {
                    AvailableRestaurantsId = table.Column<int>(type: "int", nullable: false),
                    AvailableStudnetsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantStudent", x => new { x.AvailableRestaurantsId, x.AvailableStudnetsId });
                    table.ForeignKey(
                        name: "FK_RestaurantStudent_Restaurants_AvailableRestaurantsId",
                        column: x => x.AvailableRestaurantsId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RestaurantStudent_Students_AvailableStudnetsId",
                        column: x => x.AvailableStudnetsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantStudent_AvailableStudnetsId",
                table: "RestaurantStudent",
                column: "AvailableStudnetsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RestaurantStudent");
        }
    }
}
