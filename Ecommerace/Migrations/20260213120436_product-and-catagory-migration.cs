using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerace.Migrations
{
    public partial class productandcatagorymigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateIndex(
            //    name: "IX_tbl_Product_cat_id",
            //    table: "tbl_Product",
            //    column: "cat_id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tbl_Product_tbl_Catagory_cat_id",
            //    table: "tbl_Product",
            //    column: "cat_id",
            //    principalTable: "tbl_Catagory",
            //    principalColumn: "category_id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Product_tbl_Catagory_cat_id",
                table: "tbl_Product");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Product_cat_id",
                table: "tbl_Product");
        }
    }
}
