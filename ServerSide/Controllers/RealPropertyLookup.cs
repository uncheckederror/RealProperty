using eRealProperty.Controllers;
using eRealProperty.Models;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSide.Controllers
{
    [ApiController]
    public class RealPropertyLookup : Controller
    {
        private readonly eRealPropertyContext _context;

        public RealPropertyLookup(eRealPropertyContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("RealPropertyAccounts/Search")]
        public async Task<IActionResult> IndexAsync(string Query)
        {
            var controller = new RealPropertyAccountsController(_context);

            var results = await controller.SearchForRealPropertyAccountNumbers(Query);

            if (results.Value is not null && results.Value.Any())
            {
                return Ok(results.Value);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
