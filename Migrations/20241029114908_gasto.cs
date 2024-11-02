using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orcamento.Migrations
{
    /// <inheritdoc />
    public partial class gasto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrcamentoPId",
                table: "OrcamentoP",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrcamentoP_OrcamentoPId",
                table: "OrcamentoP",
                column: "OrcamentoPId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrcamentoP_OrcamentoP_OrcamentoPId",
                table: "OrcamentoP",
                column: "OrcamentoPId",
                principalTable: "OrcamentoP",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrcamentoP_OrcamentoP_OrcamentoPId",
                table: "OrcamentoP");

            migrationBuilder.DropIndex(
                name: "IX_OrcamentoP_OrcamentoPId",
                table: "OrcamentoP");

            migrationBuilder.DropColumn(
                name: "OrcamentoPId",
                table: "OrcamentoP");
        }
    }
}
