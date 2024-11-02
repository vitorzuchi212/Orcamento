using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orcamento.Migrations
{
    /// <inheritdoc />
    public partial class FP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormaPag",
                table: "OrcamentoP",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormaPag",
                table: "OrcamentoP");
        }
    }
}
