using eRealProperty.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRealProperty.Controllers
{
    [Route("PropertyParcel")]
    [ApiController]
    public class PropertyParcelsController : Controller
    {
        private readonly eRealPropertyContext _context;

        public PropertyParcelsController(eRealPropertyContext context)
        {
            _context = context;
        }

        [HttpGet("{AccountNumber}")]
        public async Task<ActionResult<List<PropertyParcel>>> GetPropertyParcelByAccountNumber(string AccountNumber)
        {
            // Parcel Numbers are the Major followed by the Minor.
            // The last 2 digits are the current account on that parcel and should be dropped.
            var parcels = new List<PropertyParcel>();


            if (AccountNumber.Length == 10)
            {
                parcels = await _context.PropertyParcels.Where(x => x.ParcelNumber == AccountNumber).AsNoTracking().ToListAsync();

                if (parcels is null || !parcels.Any())
                {
                    var results = await _context.PropertyParcels.Where(x => x.ParcelNumber == AccountNumber).AsNoTracking().ToListAsync();

                    if (results.Any())
                    {
                        parcels = results;
                    }
                }
            }
            else if (AccountNumber.Length == 14)
            {
                parcels = await _context.PropertyParcels.Where(x => x.ParcelNumber == AccountNumber.Substring(0, 10)).AsNoTracking().ToListAsync();

                if (parcels is null || !parcels.Any())
                {
                    var results = await _context.PropertyParcels.Where(x => x.ParcelNumber == AccountNumber.Substring(0, 10)).AsNoTracking().ToListAsync();

                    if (results.Any())
                    {
                        parcels = results;
                    }
                    else
                    {
                        parcels = await _context.PropertyParcels.Where(x => x.ParcelNumber == AccountNumber.Substring(0, 10)).AsNoTracking().ToListAsync();
                    }
                }
            }
            else
            {
                parcels = await _context.PropertyParcels.Where(x => x.ParcelNumber.StartsWith(AccountNumber.Substring(0, 10))).AsNoTracking().ToListAsync();
            }

            if (!parcels.Any())
            {
                return NotFound();
            }

            return parcels;
        }
    }
}
