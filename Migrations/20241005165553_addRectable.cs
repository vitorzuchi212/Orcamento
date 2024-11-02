using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orcamento.Migrations
{
    /// <inheritdoc />
    public partial class addRectable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrcamentoPRec",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description__Rec = table.Column<string>(type: "TEXT", nullable: false),
                    Value__Rec = table.Column<double>(type: "REAL", nullable: false),
                    ActionCreate__Rec = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrcamentoPRec", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrcamentoPRec");
        }
    }
}
