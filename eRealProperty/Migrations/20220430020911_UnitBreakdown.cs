using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class UnitBreakdown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnitBreakdowns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    UnitTypeItemId = table.Column<string>(type: "TEXT", nullable: true),
                    NbrThisType = table.Column<string>(type: "TEXT", nullable: true),
                    SqFt = table.Column<string>(type: "TEXT", nullable: true),
                    NbrBedrooms = table.Column<string>(type: "TEXT", nullable: true),
                    NbrBaths = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitBreakdowns", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitBreakdowns_ParcelNumber",
                table: "UnitBreakdowns",
                column: "ParcelNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitBreakdowns");
        }
    }
}
