using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kierkels.Knaken.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NaamOmschrijving = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Rekening = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    Tegenrekening = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AfBij = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    BedragEUR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Mutatiesoort = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mededelingen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaldoNaMutatie = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
