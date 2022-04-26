using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class Permits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PermitNbr = table.Column<string>(type: "TEXT", nullable: true),
                    PermitType = table.Column<string>(type: "TEXT", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PermitVal = table.Column<int>(type: "INTEGER", nullable: false),
                    PermitStatus = table.Column<string>(type: "TEXT", nullable: true),
                    PcntComplete = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permits_ParcelNumber",
                table: "Permits",
                column: "ParcelNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permits");
        }
    }
}
