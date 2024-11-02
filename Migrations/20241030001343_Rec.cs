using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orcamento.Migrations
{
    /// <inheritdoc />
    public partial class Rec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrcamentoRecId",
                table: "OrcamentoRec",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrcamentoRec_OrcamentoRecId",
                table: "OrcamentoRec",
                column: "OrcamentoRecId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrcamentoRec_OrcamentoRec_OrcamentoRecId",
                table: "OrcamentoRec",
                column: "OrcamentoRecId",
                principalTable: "OrcamentoRec",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrcamentoRec_OrcamentoRec_OrcamentoRecId",
                table: "OrcamentoRec");

            migrationBuilder.DropIndex(
                name: "IX_OrcamentoRec_OrcamentoRecId",
                table: "OrcamentoRec");

            migrationBuilder.DropColumn(
                name: "OrcamentoRecId",
                table: "OrcamentoRec");
        }
    }
}
