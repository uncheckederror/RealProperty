using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using eRealProperty.Controllers;
using eRealProperty.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServerSide.Pages
{
    public class RealPropertyTaxYearsModel : PageModel
    {
        public eRealPropertyContext _context;
        public string ParcelNumber { get; set; }
        public List<RealPropertyAccountTaxYear> TaxYears { get; set; }

        public RealPropertyTaxYearsModel(eRealPropertyContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(string accountNumber, int taxYear)
        {
            var taxYears = new RealPropertyTaxYearsController(_context);

            ParcelNumber = accountNumber;

            if (!string.IsNullOrWhiteSpace(ParcelNumber) && ParcelNumber.Length >= 10)
            {
                var results = await taxYears.GetAllTaxYearsForASpecificAccount(ParcelNumber.Substring(0, 10));
                TaxYears = results.Value;
            }
            else if (!string.IsNullOrWhiteSpace(ParcelNumber) && (1970 < taxYear))
            {
                var results = await taxYears.GetSpecificTaxYear(ParcelNumber.Substring(0, 10), taxYear);
                TaxYears = new List<RealPropertyAccountTaxYear> { results.Value };
            }
            else
            {
                var results = await taxYears.GetAllTaxYearsForASpecificAccount("0006600106");
                TaxYears = results.Value;
            }

        }
    }
}
