#pragma warning disable IDE0053 // Use expression body for lambda expressions

namespace Hexalith.SalesHistory.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesHistory");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    InvoiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesHistory_CompanyId_InvoiceId_LineId",
                table: "SalesHistory",
                columns: new[] { "CompanyId", "InvoiceId", "LineId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesHistory_CustomerId",
                table: "SalesHistory",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesHistory_InvoiceDate",
                table: "SalesHistory",
                column: "InvoiceDate");

            migrationBuilder.CreateIndex(
                name: "IX_SalesHistory_ItemId",
                table: "SalesHistory",
                column: "ItemId");
        }
    }
}