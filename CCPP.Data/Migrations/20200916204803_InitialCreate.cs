using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CCPP.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "transactions");

            migrationBuilder.CreateTable(
                name: "PaymentTranstactions",
                schema: "transactions",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Currency = table.Column<string>(maxLength: 3, nullable: false),
                    Status = table.Column<string>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTranstactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTranstactions_Currency",
                schema: "transactions",
                table: "PaymentTranstactions",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTranstactions_Status",
                schema: "transactions",
                table: "PaymentTranstactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTranstactions_TransactionDate",
                schema: "transactions",
                table: "PaymentTranstactions",
                column: "TransactionDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentTranstactions",
                schema: "transactions");
        }
    }
}
