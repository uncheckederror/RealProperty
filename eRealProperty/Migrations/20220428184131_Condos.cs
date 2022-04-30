using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class Condos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CondoComplexes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    ComplexType = table.Column<string>(type: "TEXT", nullable: true),
                    ComplexDescr = table.Column<string>(type: "TEXT", nullable: true),
                    NbrBldgs = table.Column<int>(type: "INTEGER", nullable: false),
                    NbrStories = table.Column<int>(type: "INTEGER", nullable: false),
                    NbrUnits = table.Column<int>(type: "INTEGER", nullable: false),
                    AvgUnitSize = table.Column<int>(type: "INTEGER", nullable: false),
                    LandPerUnit = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectLocation = table.Column<string>(type: "TEXT", nullable: true),
                    ProjectAppeal = table.Column<string>(type: "TEXT", nullable: true),
                    PcntWithView = table.Column<int>(type: "INTEGER", nullable: false),
                    ConstrClass = table.Column<string>(type: "TEXT", nullable: true),
                    BldgQuality = table.Column<string>(type: "TEXT", nullable: true),
                    Condition = table.Column<string>(type: "TEXT", nullable: true),
                    YrBuilt = table.Column<int>(type: "INTEGER", nullable: false),
                    EffYr = table.Column<int>(type: "INTEGER", nullable: false),
                    PcntComplete = table.Column<int>(type: "INTEGER", nullable: false),
                    Elevators = table.Column<string>(type: "TEXT", nullable: true),
                    SectySystem = table.Column<string>(type: "TEXT", nullable: true),
                    Fireplace = table.Column<string>(type: "TEXT", nullable: true),
                    Laundry = table.Column<string>(type: "TEXT", nullable: true),
                    AptConversion = table.Column<string>(type: "TEXT", nullable: true),
                    CondoLandType = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    BuildingNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Fraction = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionPrefix = table.Column<string>(type: "TEXT", nullable: true),
                    StreetName = table.Column<string>(type: "TEXT", nullable: true),
                    StreetType = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionSuffix = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondoComplexes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CondoUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    UnitType = table.Column<string>(type: "TEXT", nullable: true),
                    BldgNbr = table.Column<string>(type: "TEXT", nullable: true),
                    UnitNbr = table.Column<string>(type: "TEXT", nullable: true),
                    PcntOwnership = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitQuality = table.Column<string>(type: "TEXT", nullable: true),
                    UnitLoc = table.Column<string>(type: "TEXT", nullable: true),
                    FloorNbr = table.Column<string>(type: "TEXT", nullable: true),
                    TopFloor = table.Column<string>(type: "TEXT", nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "TEXT", nullable: true),
                    Footage = table.Column<int>(type: "INTEGER", nullable: false),
                    NbrBedrooms = table.Column<string>(type: "TEXT", nullable: true),
                    BathFullCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BathHalfCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Bath3qtrCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Fireplace = table.Column<string>(type: "TEXT", nullable: true),
                    EndUnit = table.Column<string>(type: "TEXT", nullable: true),
                    Condition = table.Column<string>(type: "TEXT", nullable: true),
                    OtherRoom = table.Column<string>(type: "TEXT", nullable: true),
                    ViewMountain = table.Column<string>(type: "TEXT", nullable: true),
                    ViewLakeRiver = table.Column<string>(type: "TEXT", nullable: true),
                    ViewCityTerritorial = table.Column<string>(type: "TEXT", nullable: true),
                    ViewPugetSound = table.Column<string>(type: "TEXT", nullable: true),
                    ViewLakeWaSamm = table.Column<string>(type: "TEXT", nullable: true),
                    PkgOpen = table.Column<int>(type: "INTEGER", nullable: false),
                    PkgCarport = table.Column<int>(type: "INTEGER", nullable: false),
                    PkgBasement = table.Column<int>(type: "INTEGER", nullable: false),
                    PkgBasementTandem = table.Column<int>(type: "INTEGER", nullable: false),
                    PkgGarage = table.Column<int>(type: "INTEGER", nullable: false),
                    PkgGarageTandem = table.Column<int>(type: "INTEGER", nullable: false),
                    PkgOtherType = table.Column<string>(type: "TEXT", nullable: true),
                    Length = table.Column<int>(type: "INTEGER", nullable: false),
                    Width = table.Column<int>(type: "INTEGER", nullable: false),
                    YrBuilt = table.Column<int>(type: "INTEGER", nullable: false),
                    Grade = table.Column<string>(type: "TEXT", nullable: true),
                    MHomeDescr = table.Column<string>(type: "TEXT", nullable: true),
                    PersPropAcctNbr = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    BuildingNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Fraction = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionPrefix = table.Column<string>(type: "TEXT", nullable: true),
                    StreetName = table.Column<string>(type: "TEXT", nullable: true),
                    StreetType = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionSuffix = table.Column<string>(type: "TEXT", nullable: true),
                    UnitDescr = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondoUnits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CondoComplexes_Major",
                table: "CondoComplexes",
                column: "Major");

            migrationBuilder.CreateIndex(
                name: "IX_CondoUnits_ParcelNumber",
                table: "CondoUnits",
                column: "ParcelNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CondoComplexes");

            migrationBuilder.DropTable(
                name: "CondoUnits");
        }
    }
}
