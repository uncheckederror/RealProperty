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
    public class ResidentialBuildingsController : Controller
    {
        private readonly eRealPropertyContext _context;

        public ResidentialBuildingsController(eRealPropertyContext context)
        {
            _context = context;
        }

        [HttpGet("{ParcelNumber}/ResidentialBuildings")]
        public async Task<ActionResult<ResidentialBuilding[]>> GetResidentialBuildingsByParcelNumberAsync(string ParcelNumber)
        {
            if (!string.IsNullOrWhiteSpace(ParcelNumber) && ParcelNumber.Length == 10)
            {
                var buildings = await _context.ResidentialBuildings.Where(x => x.ParcelNumber == ParcelNumber).ToArrayAsync();

                if (buildings is null || !buildings.Any())
                {
                    var checkBuilding = await ResidentialBuilding.IngestByParcelNumberAsync(ParcelNumber, _context);

                    buildings = await _context.ResidentialBuildings.Where(x => x.ParcelNumber == ParcelNumber).ToArrayAsync();

                    if (buildings is null || !buildings.Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        return buildings;
                    }
                }
                else
                {
                    return buildings;
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
