using eRealProperty.Controllers;
using eRealProperty.Models;

using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Threading.Tasks;

namespace ServerSide.Pages
{
    public class LegalDescriptionModel : PageModel
    {
        public eRealPropertyContext _context;
        public string ParcelNumber { get; set; }
        public LegalDiscription[] LegalDiscriptions { get; set; }

        public LegalDescriptionModel(eRealPropertyContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(string accountNumber)
        {
            var legals = new LegalDescriptionsController(_context);

            ParcelNumber = accountNumber;

            if (!string.IsNullOrWhiteSpace(ParcelNumber) && ParcelNumber.Length >= 10)
            {
                var results = await legals.GetLegalDescriptionByParcelNumberAsync(ParcelNumber.Substring(0, 10));
                LegalDiscriptions = results.Value;
            }
            else
            {
                var results = await legals.GetLegalDescriptionByParcelNumberAsync("0006600106");
                LegalDiscriptions = results.Value;
            }
        }
    }
}
