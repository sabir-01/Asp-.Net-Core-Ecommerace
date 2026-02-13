using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerace.Migrations
{
    public partial class Cartmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Carts",
                columns: table => new
                {
                    cart_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prod_id = table.Column<int>(type: "int", nullable: false),
                    cust_id = table.Column<int>(type: "int", nullable: false),
                    product_quantity = table.Column<int>(type: "int", nullable: false),
                    cart_status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Carts", x => x.cart_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Carts");
        }
    }
}
