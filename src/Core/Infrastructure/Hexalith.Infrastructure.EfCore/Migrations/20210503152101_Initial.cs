#pragma warning disable IDE0053 // Use expression body for lambda expressions

namespace Hexalith.Infrastructure.EfCore.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Initial : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageOutbox");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "StateStreams");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageOutbox",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CausationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InProgressSince = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SentUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SystemUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageOutbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdHash = table.Column<int>(type: "int", nullable: false),
                    CreatedByUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedByUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => new { x.IdHash, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "StateStreams",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CausationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Events = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdHash = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SystemUtcDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateStreams", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageOutbox_MessageId",
                table: "MessageOutbox",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageOutbox_SentUtcDateTime",
                table: "MessageOutbox",
                column: "SentUtcDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_MessageOutbox_SessionId",
                table: "MessageOutbox",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_StateStreams_IdHash",
                table: "StateStreams",
                column: "IdHash");
        }
    }
}