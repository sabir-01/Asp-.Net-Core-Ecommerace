using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerace.Migrations
{
    public partial class orderandorderdetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<int>(type: "int", nullable: false),
                    order_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    order_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shipping_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    total_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    orderDetails_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    sub_total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.orderDetails_id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_order_id",
                        column: x => x.order_id,
                        principalTable: "Orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_tbl_Product_product_id",
                        column: x => x.product_id,
                        principalTable: "tbl_Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_tbl_Carts_cust_id",
            //    table: "tbl_Carts",
            //    column: "cust_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_tbl_Carts_prod_id",
            //    table: "tbl_Carts",
            //    column: "prod_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OrderDetails_order_id",
            //    table: "OrderDetails",
            //    column: "order_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OrderDetails_product_id",
            //    table: "OrderDetails",
            //    column: "product_id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Orders_customer_id",
            //    table: "Orders",
            //    column: "customer_id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tbl_Carts_Customers_cust_id",
            //    table: "tbl_Carts",
            //    column: "cust_id",
            //    principalTable: "Customers",
            //    principalColumn: "customer_id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_tbl_Carts_tbl_Product_prod_id",
            //    table: "tbl_Carts",
            //    column: "prod_id",
            //    principalTable: "tbl_Product",
            //    principalColumn: "product_id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Carts_Customers_cust_id",
                table: "tbl_Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Carts_tbl_Product_prod_id",
                table: "tbl_Carts");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Carts_cust_id",
                table: "tbl_Carts");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Carts_prod_id",
                table: "tbl_Carts");
        }
    }
}
