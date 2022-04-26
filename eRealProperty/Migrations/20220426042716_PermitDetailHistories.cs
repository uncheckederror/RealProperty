using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class PermitDetailHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermitDetailHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PermitNbr = table.Column<string>(type: "TEXT", nullable: true),
                    PermitItem = table.Column<string>(type: "TEXT", nullable: true),
                    ItemValue = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitDetailHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermitDetailHistories_PermitNbr",
                table: "PermitDetailHistories",
                column: "PermitNbr");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermitDetailHistories");
        }
    }
}
