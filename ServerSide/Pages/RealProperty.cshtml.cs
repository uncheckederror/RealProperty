using eRealProperty.Controllers;
using eRealProperty.Models;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSide.Pages
{
    public class RealPropertyModel : PageModel
    {
        private readonly eRealPropertyContext _context;
        public string AccountNumber { get; set; }
        public IEnumerable<RealPropertyAccount> RealPropertyAccounts { get; set; }
        public IEnumerable<RealPropertyAccountTaxYear> TaxYears { get; set; }
        public List<TaxLevy> Levies { get; set; }
        public IEnumerable<ResidentialBuilding> ResidentialBuildings { get; set; }
        public RealPropertyAccountSale[] Sales { get; set; }
        public List<PropertyParcel> Parcels { get; set; }
        public Review[] Reviews { get; set; }
        public Permit[] Permits { get; set; }
        public CommericalBuilding[] CommericalBuildings { get; set; }
        public CommericalBuildingSection[] CommericalBuildingSections { get; set; }
        public CommericalBuildingFeature[] CommericalBuildingFeatures { get; set; }
        public CondoComplex[] CondoComplexes { get; set; }
        public CondoUnit[] CondoUnits { get; set; }
        public ApartmentComplex[] ApartmentComplexes { get; set; }
        public UnitBreakdown[] UnitBreakdowns { get; set; }

        public RealPropertyModel(eRealPropertyContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(string accountNumber)
        {
            if (!string.IsNullOrWhiteSpace(accountNumber))
            {
                AccountNumber = accountNumber.Trim();

                var realProperty = new RealPropertyAccountsController(_context);

                if (AccountNumber.Length == 14)
                {
                    var results = await realProperty.GetRealPropertyAccountsByAccountNumber(AccountNumber);

                    RealPropertyAccounts = results.Value;
                }
                else
                {
                    var results = await realProperty.GetRealPropertyAccountsByAccountNumber(AccountNumber);

                    RealPropertyAccounts = results.Value;
                }

                foreach (var account in RealPropertyAccounts)
                {
                    var levys = await _context.LevyCodes.Where(x => x.LevyCode == account.LevyCode).AsNoTracking().ToListAsync();

                    if (Levies is null)
                    {
                        Levies = levys;
                    }
                    else
                    {
                        Levies.AddRange(levys);
                    }

                    var legals = new LegalDescriptionsController(_context);

                    var results = await legals.GetLegalDescriptionByParcelNumberAsync(account.ParcelNumber);

                    account.LegalDescription = results.Value.FirstOrDefault()?.LegalDesc;
                }
            }

            if (RealPropertyAccounts is not null && RealPropertyAccounts.Count() == 1)
            {
                var taxAccount = RealPropertyAccounts.FirstOrDefault();

                Parcels = await _context.PropertyParcels.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).AsNoTracking().ToListAsync();

                TaxYears = await _context.RealPropertyAccountTaxYears.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).OrderByDescending(x => x.TaxYr).AsNoTracking().ToListAsync();

                ResidentialBuildings = await _context.ResidentialBuildings.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).AsNoTracking().ToListAsync();

                Sales = await _context.Sales.Where(x => x.Major == taxAccount.Major && x.Minor == taxAccount.Minor).AsNoTracking().ToArrayAsync();

                Reviews = await _context.Reviews.Where(x => x.Major == taxAccount.Major && x.Minor == taxAccount.Minor).AsNoTracking().ToArrayAsync();

                foreach (var review in Reviews)
                {
                    review.AppealedValue = await _context.ReviewDescriptions.Where(x => x.AppealNbr == review.AppealNbr && x.ValuationType == "Appealed Value").FirstOrDefaultAsync();
                    review.FinalValue = await _context.ReviewDescriptions.Where(x => x.AppealNbr == review.AppealNbr && x.ValuationType == "Board Order Value").FirstOrDefaultAsync();
                }

                Permits = await _context.Permits.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).ToArrayAsync();

                foreach (var permit in Permits)
                {
                    var description = await _context.PermitDetailHistories.Where(x => x.PermitNbr == permit.PermitNbr && x.PermitItem == "Project Name").FirstOrDefaultAsync();
                    permit.ProjectName = !string.IsNullOrWhiteSpace(description?.ItemValue) ? description.ItemValue : string.Empty;
                }

                CommericalBuildings = await _context.CommericalBuildings.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).ToArrayAsync();
                CommericalBuildingSections = await _context.CommericalBuildingSections.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).ToArrayAsync();
                CommericalBuildingFeatures = await _context.CommericalBuildingFeatures.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).ToArrayAsync();

                CondoComplexes = await _context.CondoComplexes.Where(x => x.Major == taxAccount.Major).ToArrayAsync();
                CondoUnits = await _context.CondoUnits.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).ToArrayAsync();
                if (CondoUnits is not null && CondoUnits.Any())
                {
                    Parcels = await _context.PropertyParcels.Where(x => x.Major == taxAccount.Major).AsNoTracking().ToListAsync();
                }

                ApartmentComplexes = await _context.ApartmentComplexes.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).ToArrayAsync();
                UnitBreakdowns = await _context.UnitBreakdowns.Where(x => x.ParcelNumber == taxAccount.ParcelNumber).ToArrayAsync();
            }
        }
    }
}
