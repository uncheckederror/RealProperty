using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class AddReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AppealNbr = table.Column<string>(type: "TEXT", nullable: true),
                    EntryType = table.Column<string>(type: "TEXT", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SourcePerson = table.Column<string>(type: "TEXT", nullable: true),
                    TargetPerson = table.Column<string>(type: "TEXT", nullable: true),
                    ValuationType = table.Column<string>(type: "TEXT", nullable: true),
                    ApprLandVal = table.Column<int>(type: "INTEGER", nullable: false),
                    ApprImpsVal = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewDescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AppealNbr = table.Column<string>(type: "TEXT", nullable: true),
                    Major = table.Column<string>(type: "TEXT", nullable: true),
                    Minor = table.Column<string>(type: "TEXT", nullable: true),
                    ParcelNumber = table.Column<string>(type: "TEXT", nullable: true),
                    BillYr = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewType = table.Column<string>(type: "TEXT", nullable: true),
                    ReviewSource = table.Column<string>(type: "TEXT", nullable: true),
                    AssrRecommendation = table.Column<string>(type: "TEXT", nullable: true),
                    RespAppr = table.Column<string>(type: "TEXT", nullable: true),
                    RelatedAppealNbr = table.Column<string>(type: "TEXT", nullable: true),
                    Agent = table.Column<string>(type: "TEXT", nullable: true),
                    ValueType = table.Column<string>(type: "TEXT", nullable: true),
                    AppellantReason = table.Column<string>(type: "TEXT", nullable: true),
                    StatusAssessor = table.Column<string>(type: "TEXT", nullable: true),
                    StatusStipulation = table.Column<string>(type: "TEXT", nullable: true),
                    StatusBoard = table.Column<string>(type: "TEXT", nullable: true),
                    StatusAssmtReview = table.Column<string>(type: "TEXT", nullable: true),
                    HearingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HearingType = table.Column<string>(type: "TEXT", nullable: true),
                    HearingResult = table.Column<string>(type: "TEXT", nullable: true),
                    OrderTerm = table.Column<string>(type: "TEXT", nullable: true),
                    BoardReason = table.Column<string>(type: "TEXT", nullable: true),
                    AppealRecommended = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NoteId = table.Column<string>(type: "TEXT", nullable: true),
                    IngestedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDescriptions_AppealNbr",
                table: "ReviewDescriptions",
                column: "AppealNbr");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ParcelNumber",
                table: "Reviews",
                column: "ParcelNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewDescriptions");

            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
