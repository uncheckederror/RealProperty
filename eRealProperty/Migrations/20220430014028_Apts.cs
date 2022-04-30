using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class Apts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApartmentComplexes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    ComplexDescr = table.Column<string>(type: "TEXT", nullable: true),
                    NbrBldgs = table.Column<int>(type: "INTEGER", nullable: false),
                    NbrStories = table.Column<int>(type: "INTEGER", nullable: false),
                    NbrUnits = table.Column<int>(type: "INTEGER", nullable: false),
                    AvgUnitSize = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectLocation = table.Column<string>(type: "TEXT", nullable: true),
                    ProjectAppeal = table.Column<string>(type: "TEXT", nullable: true),
                    PcntWithView = table.Column<string>(type: "TEXT", nullable: true),
                    ConstrClass = table.Column<string>(type: "TEXT", nullable: true),
                    BldgQuality = table.Column<string>(type: "TEXT", nullable: true),
                    Condition = table.Column<string>(type: "TEXT", nullable: true),
                    YrBuilt = table.Column<int>(type: "INTEGER", nullable: false),
                    EffYr = table.Column<int>(type: "INTEGER", nullable: false),
                    PcntComplete = table.Column<string>(type: "TEXT", nullable: true),
                    Elevators = table.Column<string>(type: "TEXT", nullable: true),
                    SectySystem = table.Column<string>(type: "TEXT", nullable: true),
                    Fireplace = table.Column<string>(type: "TEXT", nullable: true),
                    Laundry = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentComplexes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentComplexes_ParcelNumber",
                table: "ApartmentComplexes",
                column: "ParcelNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentComplexes");
        }
    }
}
