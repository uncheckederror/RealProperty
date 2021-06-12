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
    public class LegalDescriptionsController : Controller
    {
        private readonly eRealPropertyContext _context;

        public LegalDescriptionsController(eRealPropertyContext context)
        {
            _context = context;
        }

        [HttpGet("{ParcelNumber}/LegalDescription")]
        public async Task<ActionResult<LegalDiscription[]>> GetLegalDescriptionByParcelNumberAsync(string ParcelNumber)
        {
            if (!string.IsNullOrWhiteSpace(ParcelNumber) && ParcelNumber.Length == 10)
            {
                var legal = await _context.LegalDiscriptions.Where(x => x.ParcelNumber == ParcelNumber).ToArrayAsync();

                if (legal is null || !legal.Any())
                {
                    var checkLegal = await LegalDiscription.IngestByParcelNumberAsync(ParcelNumber, _context);

                    legal = await _context.LegalDiscriptions.Where(x => x.ParcelNumber == ParcelNumber).ToArrayAsync();

                    if (legal is null || !legal.Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        return legal;
                    }
                }
                else
                {
                    return legal;
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
