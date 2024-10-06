using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orcamento.Migrations
{
    /// <inheritdoc />
    public partial class OrcamentoGeralTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrcamentoGeralId",
                table: "OrcamentoRec",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrcamentoGeralId",
                table: "OrcamentoP",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrcamentoGeral",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrcamentoGeral", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrcamentoRec_OrcamentoGeralId",
                table: "OrcamentoRec",
                column: "OrcamentoGeralId");

            migrationBuilder.CreateIndex(
                name: "IX_OrcamentoP_OrcamentoGeralId",
                table: "OrcamentoP",
                column: "OrcamentoGeralId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrcamentoP_OrcamentoGeral_OrcamentoGeralId",
                table: "OrcamentoP",
                column: "OrcamentoGeralId",
                principalTable: "OrcamentoGeral",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrcamentoRec_OrcamentoGeral_OrcamentoGeralId",
                table: "OrcamentoRec",
                column: "OrcamentoGeralId",
                principalTable: "OrcamentoGeral",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrcamentoP_OrcamentoGeral_OrcamentoGeralId",
                table: "OrcamentoP");

            migrationBuilder.DropForeignKey(
                name: "FK_OrcamentoRec_OrcamentoGeral_OrcamentoGeralId",
                table: "OrcamentoRec");

            migrationBuilder.DropTable(
                name: "OrcamentoGeral");

            migrationBuilder.DropIndex(
                name: "IX_OrcamentoRec_OrcamentoGeralId",
                table: "OrcamentoRec");

            migrationBuilder.DropIndex(
                name: "IX_OrcamentoP_OrcamentoGeralId",
                table: "OrcamentoP");

            migrationBuilder.DropColumn(
                name: "OrcamentoGeralId",
                table: "OrcamentoRec");

            migrationBuilder.DropColumn(
                name: "OrcamentoGeralId",
                table: "OrcamentoP");
        }
    }
}
