using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegalDiscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    LegalDesc = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalDiscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LevyCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DistrictAbbrev = table.Column<string>(type: "TEXT", nullable: true),
                    LevyCode = table.Column<string>(type: "TEXT", nullable: true),
                    DistrictName = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevyCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyParcels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PropName = table.Column<string>(type: "TEXT", nullable: true),
                    PlatName = table.Column<string>(type: "TEXT", nullable: true),
                    PlatLot = table.Column<string>(type: "TEXT", nullable: true),
                    PlatBlock = table.Column<string>(type: "TEXT", nullable: true),
                    Range = table.Column<int>(type: "INTEGER", nullable: false),
                    Township = table.Column<int>(type: "INTEGER", nullable: false),
                    Section = table.Column<int>(type: "INTEGER", nullable: false),
                    QuarterSection = table.Column<string>(type: "TEXT", nullable: true),
                    PropType = table.Column<string>(type: "TEXT", nullable: true),
                    Area = table.Column<string>(type: "TEXT", nullable: true),
                    SubArea = table.Column<string>(type: "TEXT", nullable: true),
                    SpecArea = table.Column<string>(type: "TEXT", nullable: true),
                    SpecSubArea = table.Column<string>(type: "TEXT", nullable: true),
                    DistrictName = table.Column<string>(type: "TEXT", nullable: true),
                    LevyCode = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentZoning = table.Column<string>(type: "TEXT", nullable: true),
                    HBUAsIfVacant = table.Column<string>(type: "TEXT", nullable: true),
                    HBUAsImproved = table.Column<string>(type: "TEXT", nullable: true),
                    PresentUse = table.Column<string>(type: "TEXT", nullable: true),
                    SqFtLot = table.Column<string>(type: "TEXT", nullable: true),
                    WaterSystem = table.Column<string>(type: "TEXT", nullable: true),
                    SewerSystem = table.Column<string>(type: "TEXT", nullable: true),
                    Access = table.Column<string>(type: "TEXT", nullable: true),
                    Topography = table.Column<string>(type: "TEXT", nullable: true),
                    StreetSurface = table.Column<string>(type: "TEXT", nullable: true),
                    RestrictiveSzShape = table.Column<string>(type: "TEXT", nullable: true),
                    InadequateParking = table.Column<string>(type: "TEXT", nullable: true),
                    PcntUnusable = table.Column<string>(type: "TEXT", nullable: true),
                    Unbuildable = table.Column<string>(type: "TEXT", nullable: true),
                    MtRainier = table.Column<string>(type: "TEXT", nullable: true),
                    Olympics = table.Column<string>(type: "TEXT", nullable: true),
                    Cascades = table.Column<string>(type: "TEXT", nullable: true),
                    Territorial = table.Column<string>(type: "TEXT", nullable: true),
                    SeattleSkyline = table.Column<string>(type: "TEXT", nullable: true),
                    PugetSound = table.Column<string>(type: "TEXT", nullable: true),
                    LakeWashington = table.Column<string>(type: "TEXT", nullable: true),
                    LakeSammamish = table.Column<string>(type: "TEXT", nullable: true),
                    SmallLakeRiverCreek = table.Column<string>(type: "TEXT", nullable: true),
                    OtherView = table.Column<string>(type: "TEXT", nullable: true),
                    WfntLocation = table.Column<string>(type: "TEXT", nullable: true),
                    WfntFootage = table.Column<string>(type: "TEXT", nullable: true),
                    WfntBank = table.Column<string>(type: "TEXT", nullable: true),
                    WfntPoorQuality = table.Column<string>(type: "TEXT", nullable: true),
                    WfntRestrictedAccess = table.Column<string>(type: "TEXT", nullable: true),
                    WfntAccessRights = table.Column<string>(type: "TEXT", nullable: true),
                    WfntProximityInfluence = table.Column<string>(type: "TEXT", nullable: true),
                    TidelandShoreland = table.Column<string>(type: "TEXT", nullable: true),
                    LotDepthFactor = table.Column<string>(type: "TEXT", nullable: true),
                    TrafficNoise = table.Column<string>(type: "TEXT", nullable: true),
                    AirportNoise = table.Column<string>(type: "TEXT", nullable: true),
                    PowerLines = table.Column<string>(type: "TEXT", nullable: true),
                    OtherNuisances = table.Column<string>(type: "TEXT", nullable: true),
                    NbrBldgSites = table.Column<string>(type: "TEXT", nullable: true),
                    Contamination = table.Column<string>(type: "TEXT", nullable: true),
                    DNRLease = table.Column<string>(type: "TEXT", nullable: true),
                    AdjacentGolfFairway = table.Column<string>(type: "TEXT", nullable: true),
                    AdjacentGreenbelt = table.Column<string>(type: "TEXT", nullable: true),
                    HistoricSite = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentUseDesignation = table.Column<string>(type: "TEXT", nullable: true),
                    NativeGrowthProtEsmt = table.Column<string>(type: "TEXT", nullable: true),
                    Easements = table.Column<string>(type: "TEXT", nullable: true),
                    OtherDesignation = table.Column<string>(type: "TEXT", nullable: true),
                    DeedRestrictions = table.Column<string>(type: "TEXT", nullable: true),
                    DevelopmentRightsPurch = table.Column<string>(type: "TEXT", nullable: true),
                    CoalMineHazard = table.Column<string>(type: "TEXT", nullable: true),
                    CriticalDrainage = table.Column<string>(type: "TEXT", nullable: true),
                    ErosionHazard = table.Column<string>(type: "TEXT", nullable: true),
                    LandfillBuffer = table.Column<string>(type: "TEXT", nullable: true),
                    HundredYrFloodPlain = table.Column<string>(type: "TEXT", nullable: true),
                    SeismicHazard = table.Column<string>(type: "TEXT", nullable: true),
                    LandslideHazard = table.Column<string>(type: "TEXT", nullable: true),
                    SteepSlopeHazard = table.Column<string>(type: "TEXT", nullable: true),
                    Stream = table.Column<string>(type: "TEXT", nullable: true),
                    Wetland = table.Column<string>(type: "TEXT", nullable: true),
                    SpeciesOfConcern = table.Column<string>(type: "TEXT", nullable: true),
                    SensitiveAreaTract = table.Column<string>(type: "TEXT", nullable: true),
                    WaterProblems = table.Column<string>(type: "TEXT", nullable: true),
                    TranspConcurrency = table.Column<string>(type: "TEXT", nullable: true),
                    OtherProblems = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyParcels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealPropertyAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AcctNbr = table.Column<string>(type: "TEXT", nullable: true),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    AttnLine = table.Column<string>(type: "TEXT", nullable: true),
                    AddrLine = table.Column<string>(type: "TEXT", nullable: true),
                    CityState = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    LevyCode = table.Column<string>(type: "TEXT", nullable: true),
                    TaxStat = table.Column<string>(type: "TEXT", nullable: true),
                    BillYr = table.Column<int>(type: "INTEGER", nullable: false),
                    NewConstructionFlag = table.Column<string>(type: "TEXT", nullable: true),
                    TaxValReason = table.Column<string>(type: "TEXT", nullable: true),
                    ApprLandVal = table.Column<string>(type: "TEXT", nullable: true),
                    ApprImpsVal = table.Column<string>(type: "TEXT", nullable: true),
                    TaxableLandVal = table.Column<string>(type: "TEXT", nullable: true),
                    TaxableImpsVal = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LegalDescription = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealPropertyAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealPropertyAccountTaxYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    TaxYr = table.Column<int>(type: "INTEGER", nullable: false),
                    OmitYr = table.Column<int>(type: "INTEGER", nullable: false),
                    ApprLandVal = table.Column<long>(type: "INTEGER", nullable: false),
                    ApprImpsVal = table.Column<long>(type: "INTEGER", nullable: false),
                    ApprImpIncr = table.Column<long>(type: "INTEGER", nullable: false),
                    LandVal = table.Column<long>(type: "INTEGER", nullable: false),
                    ImpsVal = table.Column<long>(type: "INTEGER", nullable: false),
                    TaxValReason = table.Column<string>(type: "TEXT", nullable: true),
                    TaxStat = table.Column<string>(type: "TEXT", nullable: true),
                    LevyCode = table.Column<int>(type: "INTEGER", nullable: false),
                    ChangeDate = table.Column<string>(type: "TEXT", nullable: true),
                    ChangeDocId = table.Column<string>(type: "TEXT", nullable: true),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    SplitCode = table.Column<char>(type: "TEXT", nullable: false),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealPropertyAccountTaxYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResidentialBuildings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    BldgNbr = table.Column<int>(type: "INTEGER", nullable: false),
                    NbrLivingUnits = table.Column<int>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    BuildingNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Fraction = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionPrefix = table.Column<string>(type: "TEXT", nullable: true),
                    StreetName = table.Column<string>(type: "TEXT", nullable: true),
                    StreetType = table.Column<string>(type: "TEXT", nullable: true),
                    DirectionSuffix = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<string>(type: "TEXT", nullable: true),
                    Stories = table.Column<string>(type: "TEXT", nullable: true),
                    BldgGrade = table.Column<string>(type: "TEXT", nullable: true),
                    BldgGradeVar = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFt1stFloor = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtHalfFloor = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFt2ndFloor = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtUpperFloor = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtUnfinFull = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtUnfinHalf = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtTotLiving = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtTotBasement = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtFinBasement = table.Column<int>(type: "INTEGER", nullable: false),
                    FinBasementGrade = table.Column<string>(type: "TEXT", nullable: true),
                    SqFtGarageBasement = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtGarageAttached = table.Column<int>(type: "INTEGER", nullable: false),
                    DaylightBasement = table.Column<string>(type: "TEXT", nullable: true),
                    SqFtOpenPorch = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtEnclosedPorch = table.Column<int>(type: "INTEGER", nullable: false),
                    SqFtDeck = table.Column<int>(type: "INTEGER", nullable: false),
                    HeatSystem = table.Column<string>(type: "TEXT", nullable: true),
                    HeatSource = table.Column<string>(type: "TEXT", nullable: true),
                    BrickStone = table.Column<int>(type: "INTEGER", nullable: false),
                    ViewUtilization = table.Column<string>(type: "TEXT", nullable: true),
                    Bedrooms = table.Column<int>(type: "INTEGER", nullable: false),
                    BathHalfCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Bath3qtrCount = table.Column<int>(type: "INTEGER", nullable: false),
                    BathFullCount = table.Column<int>(type: "INTEGER", nullable: false),
                    FpSingleStory = table.Column<int>(type: "INTEGER", nullable: false),
                    FpMultiStory = table.Column<int>(type: "INTEGER", nullable: false),
                    FpFreestanding = table.Column<int>(type: "INTEGER", nullable: false),
                    FpAdditional = table.Column<int>(type: "INTEGER", nullable: false),
                    YrBuilt = table.Column<int>(type: "INTEGER", nullable: false),
                    YrRenovated = table.Column<int>(type: "INTEGER", nullable: false),
                    PcntComplete = table.Column<int>(type: "INTEGER", nullable: false),
                    Obsolescence = table.Column<int>(type: "INTEGER", nullable: false),
                    PcntNetCondition = table.Column<int>(type: "INTEGER", nullable: false),
                    Condition = table.Column<string>(type: "TEXT", nullable: true),
                    AddnlCost = table.Column<int>(type: "INTEGER", nullable: false),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidentialBuildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExciseTaxNbr = table.Column<int>(type: "INTEGER", nullable: false),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SalePrice = table.Column<long>(type: "INTEGER", nullable: false),
                    RecordingNbr = table.Column<string>(type: "TEXT", nullable: true),
                    Volume = table.Column<string>(type: "TEXT", nullable: true),
                    Page = table.Column<string>(type: "TEXT", nullable: true),
                    PlatNbr = table.Column<string>(type: "TEXT", nullable: true),
                    PlatType = table.Column<string>(type: "TEXT", nullable: true),
                    PlatLot = table.Column<string>(type: "TEXT", nullable: true),
                    PlatBlock = table.Column<string>(type: "TEXT", nullable: true),
                    SellerName = table.Column<string>(type: "TEXT", nullable: true),
                    BuyerName = table.Column<string>(type: "TEXT", nullable: true),
                    PropertyType = table.Column<string>(type: "TEXT", nullable: true),
                    PrincipalUse = table.Column<string>(type: "TEXT", nullable: true),
                    SaleInstrument = table.Column<string>(type: "TEXT", nullable: true),
                    AFForestLand = table.Column<char>(type: "TEXT", nullable: false),
                    AFCurrentUseLand = table.Column<char>(type: "TEXT", nullable: false),
                    AFNonProfitUse = table.Column<char>(type: "TEXT", nullable: false),
                    AFHistoricProperty = table.Column<char>(type: "TEXT", nullable: false),
                    SaleReason = table.Column<string>(type: "TEXT", nullable: true),
                    PropertyClass = table.Column<string>(type: "TEXT", nullable: true),
                    SaleWarning = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegalDiscriptions_ParcelNumber",
                table: "LegalDiscriptions",
                column: "ParcelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_LevyCodes_LevyCode",
                table: "LevyCodes",
                column: "LevyCode");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyParcels_ParcelNumber",
                table: "PropertyParcels",
                column: "ParcelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_RealPropertyAccounts_AcctNbr",
                table: "RealPropertyAccounts",
                column: "AcctNbr");

            migrationBuilder.CreateIndex(
                name: "IX_RealPropertyAccountTaxYears_ParcelNumber",
                table: "RealPropertyAccountTaxYears",
                column: "ParcelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_ResidentialBuildings_ParcelNumber",
                table: "ResidentialBuildings",
                column: "ParcelNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ParcelNumber",
                table: "Sales",
                column: "ParcelNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalDiscriptions");

            migrationBuilder.DropTable(
                name: "LevyCodes");

            migrationBuilder.DropTable(
                name: "PropertyParcels");

            migrationBuilder.DropTable(
                name: "RealPropertyAccounts");

            migrationBuilder.DropTable(
                name: "RealPropertyAccountTaxYears");

            migrationBuilder.DropTable(
                name: "ResidentialBuildings");

            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
