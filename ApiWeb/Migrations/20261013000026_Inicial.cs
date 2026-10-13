using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiWeb.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entrenadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Apellido = table.Column<string>(type: "TEXT", nullable: false),
                    Especialidad = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrenadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maquinarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Musculo = table.Column<string>(type: "TEXT", nullable: false),
                    Estado = table.Column<string>(type: "TEXT", nullable: false),
                    FechaUltimoMantenimiento = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinarias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Membresias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membresias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rutinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    NivelDificultad = table.Column<string>(type: "TEXT", nullable: false),
                    EntrenadorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rutinas_Entrenadores_EntrenadorId",
                        column: x => x.EntrenadorId,
                        principalTable: "Entrenadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Miembros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Apellido = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    MembresiaId = table.Column<int>(type: "INTEGER", nullable: false),
                    RutinaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miembros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Miembros_Membresias_MembresiaId",
                        column: x => x.MembresiaId,
                        principalTable: "Membresias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Miembros_Rutinas_RutinaId",
                        column: x => x.RutinaId,
                        principalTable: "Rutinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaPago = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Monto = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    MiembroId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Miembros_MiembroId",
                        column: x => x.MiembroId,
                        principalTable: "Miembros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Entrenadores",
                columns: new[] { "Id", "Apellido", "Especialidad", "Nombre" },
                values: new object[,]
                {
                    { 1, "Fernández", "Musculación", "Lucas" },
                    { 2, "Gómez", "Funcional", "Sofía" }
                });

            migrationBuilder.InsertData(
                table: "Maquinarias",
                columns: new[] { "Id", "Estado", "FechaUltimoMantenimiento", "Musculo", "Nombre" },
                values: new object[,]
                {
                    { 1, "Operativa", new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pecho", "Press de banca" },
                    { 2, "Operativa", new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Espalda", "Polea alta" },
                    { 3, "En mantenimiento", new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Piernas", "Prensa de piernas" }
                });

            migrationBuilder.InsertData(
                table: "Membresias",
                columns: new[] { "Id", "Precio", "Tipo" },
                values: new object[,]
                {
                    { 1, 25000m, "Mensual" },
                    { 2, 65000m, "Trimestral" },
                    { 3, 230000m, "Anual" }
                });

            migrationBuilder.InsertData(
                table: "Rutinas",
                columns: new[] { "Id", "Descripcion", "EntrenadorId", "NivelDificultad" },
                values: new object[,]
                {
                    { 1, "Full body 3 días", 1, "Principiante" },
                    { 2, "Pecho y espalda", 1, "Intermedio" },
                    { 3, "Circuito funcional", 2, "Intermedio" }
                });

            migrationBuilder.InsertData(
                table: "Miembros",
                columns: new[] { "Id", "Apellido", "Email", "MembresiaId", "Nombre", "RutinaId" },
                values: new object[,]
                {
                    { 1, "Pérez", "martin@mail.com", 1, "Martín", 1 },
                    { 2, "López", "camila@mail.com", 2, "Camila", 3 }
                });

            migrationBuilder.InsertData(
                table: "Pagos",
                columns: new[] { "Id", "FechaPago", "MiembroId", "Monto" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 25000m },
                    { 2, new DateTime(2026, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 65000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Miembros_Email",
                table: "Miembros",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Miembros_MembresiaId",
                table: "Miembros",
                column: "MembresiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Miembros_RutinaId",
                table: "Miembros",
                column: "RutinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_MiembroId",
                table: "Pagos",
                column: "MiembroId");

            migrationBuilder.CreateIndex(
                name: "IX_Rutinas_EntrenadorId",
                table: "Rutinas",
                column: "EntrenadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Maquinarias");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Miembros");

            migrationBuilder.DropTable(
                name: "Membresias");

            migrationBuilder.DropTable(
                name: "Rutinas");

            migrationBuilder.DropTable(
                name: "Entrenadores");
        }
    }
}
