using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSVenta.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cliente",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nchar(50)", fixedLength: true, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cliente", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "producto",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nchar(150)", fixedLength: true, maxLength: 150, nullable: false),
                    precioUnitario = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(16,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_producto", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venta",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    total = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venta", x => x.id);
                    table.ForeignKey(
                        name: "FK_venta_cliente",
                        column: x => x.id_cliente,
                        principalTable: "cliente",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "concepto",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venta = table.Column<long>(type: "bigint", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    precioUnitario = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    importe = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    id_producto = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_concepto", x => x.id);
                    table.ForeignKey(
                        name: "FK_Concepto_producto",
                        column: x => x.id_producto,
                        principalTable: "producto",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_concepto_Venta",
                        column: x => x.id_venta,
                        principalTable: "venta",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_concepto_id_producto",
                table: "concepto",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_concepto_id_venta",
                table: "concepto",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_venta_id_cliente",
                table: "venta",
                column: "id_cliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "concepto");

            migrationBuilder.DropTable(
                name: "producto");

            migrationBuilder.DropTable(
                name: "venta");

            migrationBuilder.DropTable(
                name: "cliente");
        }
    }
}
