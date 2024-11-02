using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orcamento.Migrations
{
    /// <inheritdoc />
    public partial class tableorcamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoOperacao",
                table: "OrcamentoRec",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoOperacao",
                table: "OrcamentoP",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoOperacao",
                table: "OrcamentoRec");

            migrationBuilder.DropColumn(
                name: "TipoOperacao",
                table: "OrcamentoP");
        }
    }
}
