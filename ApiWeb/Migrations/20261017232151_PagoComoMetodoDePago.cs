using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWeb.Migrations
{
    /// <inheritdoc />
    public partial class PagoComoMetodoDePago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Miembros_MiembroId",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_MiembroId",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "MiembroId",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "Monto",
                table: "Pagos");

            migrationBuilder.RenameColumn(
                name: "FechaPago",
                table: "Pagos",
                newName: "Metodo");

            migrationBuilder.AddColumn<int>(
                name: "PagoId",
                table: "Miembros",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Miembros",
                keyColumn: "Id",
                keyValue: 1,
                column: "PagoId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Miembros",
                keyColumn: "Id",
                keyValue: 2,
                column: "PagoId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Pagos",
                keyColumn: "Id",
                keyValue: 1,
                column: "Metodo",
                value: "Efectivo");

            migrationBuilder.UpdateData(
                table: "Pagos",
                keyColumn: "Id",
                keyValue: 2,
                column: "Metodo",
                value: "Transferencia");

            migrationBuilder.CreateIndex(
                name: "IX_Miembros_PagoId",
                table: "Miembros",
                column: "PagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Miembros_Pagos_PagoId",
                table: "Miembros",
                column: "PagoId",
                principalTable: "Pagos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Miembros_Pagos_PagoId",
                table: "Miembros");

            migrationBuilder.DropIndex(
                name: "IX_Miembros_PagoId",
                table: "Miembros");

            migrationBuilder.DropColumn(
                name: "PagoId",
                table: "Miembros");

            migrationBuilder.RenameColumn(
                name: "Metodo",
                table: "Pagos",
                newName: "FechaPago");

            migrationBuilder.AddColumn<int>(
                name: "MiembroId",
                table: "Pagos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Monto",
                table: "Pagos",
                type: "TEXT",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Pagos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaPago", "MiembroId", "Monto" },
                values: new object[] { new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 25000m });

            migrationBuilder.UpdateData(
                table: "Pagos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaPago", "MiembroId", "Monto" },
                values: new object[] { new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 65000m });

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_MiembroId",
                table: "Pagos",
                column: "MiembroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Miembros_MiembroId",
                table: "Pagos",
                column: "MiembroId",
                principalTable: "Miembros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
