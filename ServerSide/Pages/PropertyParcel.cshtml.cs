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
    public class PropertyParcelModel : PageModel
    {
        private readonly eRealPropertyContext _context;
        public string AccountNumber { get; set; }
        public List<PropertyParcel> Parcels { get; set; }

        public PropertyParcelModel(eRealPropertyContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(string accountNumber)
        {
            if (!string.IsNullOrWhiteSpace(accountNumber) && accountNumber.Length >= 10)
            {
                AccountNumber = accountNumber.Trim().Substring(0, 10);

                var parcels = new PropertyParcelsController(_context);

                var results = await parcels.GetPropertyParcelByAccountNumber(AccountNumber);

                Parcels = results.Value;
            }
        }
    }
}
