using eRealProperty.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRealProperty.Controllers
{
    [Route("RealAccount")]
    [ApiController]
    public class RealPropertySalesController : Controller
    {
        private readonly eRealPropertyContext _context;

        public RealPropertySalesController(eRealPropertyContext context)
        {
            _context = context;
        }

        [HttpGet("{ParcelNumber}/Sales")]
        public async Task<ActionResult<RealPropertyAccountSale[]>> GetSalesByParcelAsync(string ParcelNumber)
        {
            if (!string.IsNullOrWhiteSpace(ParcelNumber) && ParcelNumber.Length == 10)
            {
                var sales = await _context.Sales.Where(x => x.ParcelNumber == ParcelNumber).ToArrayAsync();

                if (sales is null || !sales.Any())
                {
                    await RealPropertyAccountSale.IngestByParcelNumberAsync(ParcelNumber, _context);

                    sales = await _context.Sales.Where(x => x.ParcelNumber == ParcelNumber).ToArrayAsync();

                    if (sales is null || !sales.Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        return sales;
                    }
                }
                else
                {
                    return sales;
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
