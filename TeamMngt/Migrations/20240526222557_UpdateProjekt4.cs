using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamMngt.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjekt4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pracownik_Zespol_ZespolId",
                table: "Pracownik");

            migrationBuilder.DropForeignKey(
                name: "FK_Zadanie_ModulProjektu_ModulProjektuId",
                table: "Zadanie");

            migrationBuilder.DropForeignKey(
                name: "FK_Zespol_ModulProjektu_ModulProjektuId",
                table: "Zespol");

            migrationBuilder.AddForeignKey(
                name: "FK_Pracownik_Zespol_ZespolId",
                table: "Pracownik",
                column: "ZespolId",
                principalTable: "Zespol",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Zadanie_ModulProjektu_ModulProjektuId",
                table: "Zadanie",
                column: "ModulProjektuId",
                principalTable: "ModulProjektu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zespol_ModulProjektu_ModulProjektuId",
                table: "Zespol",
                column: "ModulProjektuId",
                principalTable: "ModulProjektu",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pracownik_Zespol_ZespolId",
                table: "Pracownik");

            migrationBuilder.DropForeignKey(
                name: "FK_Zadanie_ModulProjektu_ModulProjektuId",
                table: "Zadanie");

            migrationBuilder.DropForeignKey(
                name: "FK_Zespol_ModulProjektu_ModulProjektuId",
                table: "Zespol");

            migrationBuilder.AddForeignKey(
                name: "FK_Pracownik_Zespol_ZespolId",
                table: "Pracownik",
                column: "ZespolId",
                principalTable: "Zespol",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Zadanie_ModulProjektu_ModulProjektuId",
                table: "Zadanie",
                column: "ModulProjektuId",
                principalTable: "ModulProjektu",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Zespol_ModulProjektu_ModulProjektuId",
                table: "Zespol",
                column: "ModulProjektuId",
                principalTable: "ModulProjektu",
                principalColumn: "Id");
        }
    }
}
