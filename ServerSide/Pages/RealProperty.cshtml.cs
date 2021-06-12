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
                var parcelQuery = RealPropertyAccounts.FirstOrDefault().ParcelNumber;

                using var parcels = new PropertyParcelsController(_context);

                var results = await parcels.GetPropertyParcelByAccountNumber(parcelQuery);

                Parcels = results.Value;

                using var taxYears = new RealPropertyTaxYearsController(_context);

                var years = await taxYears.GetAllTaxYearsForASpecificAccount(parcelQuery);

                TaxYears = years.Value;

                using var residentialBuildings = new ResidentialBuildingsController(_context);

                var buildings = await residentialBuildings.GetResidentialBuildingsByParcelNumberAsync(parcelQuery);

                ResidentialBuildings = buildings.Value;

                using var realaccountSales = new RealPropertySalesController(_context);

                var sales = await realaccountSales.GetSalesByParcelAsync(parcelQuery);

                Sales = sales.Value;
            }
        }
    }
}
