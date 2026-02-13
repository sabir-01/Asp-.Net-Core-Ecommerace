using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerace.Migrations
{
    public partial class Productmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Product",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cat_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Product", x => x.product_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Product");
        }
    }
}
