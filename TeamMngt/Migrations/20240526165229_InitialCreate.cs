using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamMngt.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projekt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    DataRozpoczecia = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Opis = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projekt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModulProjektu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    DataRozpoczecia = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Opis = table.Column<string>(type: "TEXT", nullable: true),
                    ProjektId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulProjektu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModulProjektu_Projekt_ProjektId",
                        column: x => x.ProjektId,
                        principalTable: "Projekt",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Zespol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    Opis = table.Column<string>(type: "TEXT", nullable: true),
                    ModulProjektuId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zespol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zespol_ModulProjektu_ModulProjektuId",
                        column: x => x.ModulProjektuId,
                        principalTable: "ModulProjektu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pracownik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    Stanowsiko = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    ZespolId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pracownik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pracownik_Zespol_ZespolId",
                        column: x => x.ZespolId,
                        principalTable: "Zespol",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Zadanie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    CzasWykonania = table.Column<decimal>(type: "TEXT", nullable: false),
                    Deadline = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Opis = table.Column<string>(type: "TEXT", nullable: true),
                    ModulProjektuId = table.Column<int>(type: "INTEGER", nullable: true),
                    PracownikId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zadanie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zadanie_ModulProjektu_ModulProjektuId",
                        column: x => x.ModulProjektuId,
                        principalTable: "ModulProjektu",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Zadanie_Pracownik_PracownikId",
                        column: x => x.PracownikId,
                        principalTable: "Pracownik",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModulProjektu_ProjektId",
                table: "ModulProjektu",
                column: "ProjektId");

            migrationBuilder.CreateIndex(
                name: "IX_Pracownik_ZespolId",
                table: "Pracownik",
                column: "ZespolId");

            migrationBuilder.CreateIndex(
                name: "IX_Zadanie_ModulProjektuId",
                table: "Zadanie",
                column: "ModulProjektuId");

            migrationBuilder.CreateIndex(
                name: "IX_Zadanie_PracownikId",
                table: "Zadanie",
                column: "PracownikId");

            migrationBuilder.CreateIndex(
                name: "IX_Zespol_ModulProjektuId",
                table: "Zespol",
                column: "ModulProjektuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zadanie");

            migrationBuilder.DropTable(
                name: "Pracownik");

            migrationBuilder.DropTable(
                name: "Zespol");

            migrationBuilder.DropTable(
                name: "ModulProjektu");

            migrationBuilder.DropTable(
                name: "Projekt");
        }
    }
}
