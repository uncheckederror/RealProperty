using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eRealProperty.Migrations
{
    public partial class ChangeTaxStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxStat",
                table: "RealPropertyAccountTaxYears",
                newName: "TaxStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxStatus",
                table: "RealPropertyAccountTaxYears",
                newName: "TaxStat");
        }
    }
}
