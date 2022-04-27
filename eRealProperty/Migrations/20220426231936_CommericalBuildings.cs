using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class CommericalBuildings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommericalBuildingFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    BldgNbr = table.Column<string>(type: "TEXT", nullable: true),
                    FeatureType = table.Column<string>(type: "TEXT", nullable: true),
                    GrossSqFt = table.Column<string>(type: "TEXT", nullable: true),
                    NetSqFt = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommericalBuildingFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommericalBuildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    BldgNbr = table.Column<string>(type: "TEXT", nullable: true),
                    NbrBldgs = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    BuildingNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Fraction = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionPrefix = table.Column<string>(type: "TEXT", nullable: true),
                    StreetName = table.Column<string>(type: "TEXT", nullable: true),
                    StreetType = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionSuffix = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    NbrStories = table.Column<string>(type: "TEXT", nullable: true),
                    PredominantUse = table.Column<string>(type: "TEXT", nullable: true),
                    Shape = table.Column<string>(type: "TEXT", nullable: true),
                    ConstrClass = table.Column<string>(type: "TEXT", nullable: true),
                    BldgQuality = table.Column<string>(type: "TEXT", nullable: true),
                    BldgDescr = table.Column<string>(type: "TEXT", nullable: true),
                    BldgGrossSqFt = table.Column<string>(type: "TEXT", nullable: true),
                    BldgNetSqFt = table.Column<string>(type: "TEXT", nullable: true),
                    YrBuilt = table.Column<string>(type: "TEXT", nullable: true),
                    EffYr = table.Column<string>(type: "TEXT", nullable: true),
                    PcntComplete = table.Column<string>(type: "TEXT", nullable: true),
                    HeatingSystem = table.Column<string>(type: "TEXT", nullable: true),
                    Sprinklers = table.Column<string>(type: "TEXT", nullable: true),
                    Elevators = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommericalBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommericalBuildingSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    BldgNbr = table.Column<string>(type: "TEXT", nullable: true),
                    SectionNbr = table.Column<string>(type: "TEXT", nullable: true),
                    SectionUse = table.Column<string>(type: "TEXT", nullable: true),
                    NbrStories = table.Column<string>(type: "TEXT", nullable: true),
                    StoryHeight = table.Column<string>(type: "TEXT", nullable: true),
                    GrossSqFt = table.Column<string>(type: "TEXT", nullable: true),
                    NetSqFt = table.Column<string>(type: "TEXT", nullable: true),
                    SectionDescr = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommericalBuildingSections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommericalBuildingFeatures_ParcelNumber",
                table: "CommericalBuildingFeatures",
                column: "ParcelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_CommericalBuildings_ParcelNumber",
                table: "CommericalBuildings",
                column: "ParcelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_CommericalBuildingSections_ParcelNumber",
                table: "CommericalBuildingSections",
                column: "ParcelNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommericalBuildingFeatures");

            migrationBuilder.DropTable(
                name: "CommericalBuildings");

            migrationBuilder.DropTable(
                name: "CommericalBuildingSections");
        }
    }
}
