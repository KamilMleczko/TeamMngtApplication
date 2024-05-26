using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamMngt.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjekt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModulProjektu_Projekt_ProjektId",
                table: "ModulProjektu");

            migrationBuilder.AddForeignKey(
                name: "FK_ModulProjektu_Projekt_ProjektId",
                table: "ModulProjektu",
                column: "ProjektId",
                principalTable: "Projekt",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModulProjektu_Projekt_ProjektId",
                table: "ModulProjektu");

            migrationBuilder.AddForeignKey(
                name: "FK_ModulProjektu_Projekt_ProjektId",
                table: "ModulProjektu",
                column: "ProjektId",
                principalTable: "Projekt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
