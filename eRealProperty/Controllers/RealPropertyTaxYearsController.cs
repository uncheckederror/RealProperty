using eRealProperty.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRealProperty.Controllers
{
    [Route("RealProperty")]
    [ApiController]
    public class RealPropertyTaxYearsController : Controller
    {
        private readonly eRealPropertyContext _context;

        public RealPropertyTaxYearsController(eRealPropertyContext context)
        {
            _context = context;
        }

        [HttpGet("TaxYears")]
        public async Task<ActionResult<List<int>>> GetAllTaxYears()
        {
            var years = await _context.RealPropertyAccountTaxYears.Select(x => x.TaxYr).Distinct().OrderByDescending(x => x).ToListAsync();

            return Ok(years);
        }

        [HttpGet("{parcelNumber}/TaxYears")]
        public async Task<ActionResult<List<RealPropertyAccountTaxYear>>> GetAllTaxYearsForASpecificAccount(string parcelNumber)
        {
            var realPropertyTaxYear = await _context.RealPropertyAccountTaxYears.Where(x => x.ParcelNumber == parcelNumber).OrderByDescending(x => x.TaxYr).AsNoTracking().ToListAsync();

            if (realPropertyTaxYear is null || !realPropertyTaxYear.Any())
            {
                return NotFound();
            }
            else
            {
                return realPropertyTaxYear;
            }
        }

        [HttpGet("{parcelNumber}/TaxYear/{taxYear}")]
        public async Task<ActionResult<RealPropertyAccountTaxYear>> GetSpecificTaxYear(string parcelNumber, int taxYear)
        {
            var realPropertyTaxYear = await _context.RealPropertyAccountTaxYears.Where(x => x.ParcelNumber == parcelNumber && x.TaxYr == taxYear).AsNoTracking().FirstOrDefaultAsync();

            if (realPropertyTaxYear is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(realPropertyTaxYear);
            }
        }
    }
}
