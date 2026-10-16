using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiWeb.Migrations
{
    /// <inheritdoc />
    public partial class AgregoMembresiaVip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Membresias",
                keyColumn: "Id",
                keyValue: 1,
                column: "Tipo",
                value: "Basico");

            migrationBuilder.UpdateData(
                table: "Membresias",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Precio", "Tipo" },
                values: new object[] { 45000m, "Estandar" });

            migrationBuilder.UpdateData(
                table: "Membresias",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Precio", "Tipo" },
                values: new object[] { 80000m, "Avanzado" });

            migrationBuilder.InsertData(
                table: "Membresias",
                columns: new[] { "Id", "Precio", "Tipo" },
                values: new object[] { 4, 100000m, "Vip" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Membresias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Membresias",
                keyColumn: "Id",
                keyValue: 1,
                column: "Tipo",
                value: "Mensual");

            migrationBuilder.UpdateData(
                table: "Membresias",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Precio", "Tipo" },
                values: new object[] { 65000m, "Trimestral" });

            migrationBuilder.UpdateData(
                table: "Membresias",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Precio", "Tipo" },
                values: new object[] { 230000m, "Anual" });
        }
    }
}
