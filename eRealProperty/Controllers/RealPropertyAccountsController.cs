using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eRealProperty.Models;

namespace eRealProperty.Controllers
{
    [Route("RealProperty")]
    [ApiController]
    public class RealPropertyAccountsController : ControllerBase
    {
        private readonly eRealPropertyContext _context;

        public RealPropertyAccountsController(eRealPropertyContext context)
        {
            _context = context;
        }

        [HttpGet("Accounts")]
        public async Task<ActionResult<List<string>>> GetAllAccountNumbersAsync()
        {
            var realPropertyAccount = await _context.RealPropertyAccounts.Select(x => x.AcctNbr).AsNoTracking().ToListAsync();

            return realPropertyAccount;
        }

        [HttpGet("{AccountNumber}")]
        public async Task<ActionResult<List<RealPropertyAccount>>> GetRealPropertyAccountsByAccountNumber(string AccountNumber)
        {
            var realPropertyAccounts = new List<RealPropertyAccount>();

            if (AccountNumber.Length <= 6)
            {
                realPropertyAccounts = await _context.RealPropertyAccounts.Where(x => x.Major.StartsWith(AccountNumber)).AsNoTracking().ToListAsync();
            }
            else if (AccountNumber.Length <= 10)
            {
                var major = AccountNumber.Substring(0, 6);
                var minor = AccountNumber.Substring(6, AccountNumber.Length - 6);
                realPropertyAccounts = await _context.RealPropertyAccounts.Where(x => (x.Major == major) && (x.Minor.StartsWith(minor))).AsNoTracking().ToListAsync();
            }
            else if (AccountNumber.Length == 14)
            {
                realPropertyAccounts = await _context.RealPropertyAccounts.Where(x => x.AcctNbr == AccountNumber).AsNoTracking().ToListAsync();
            }
            else
            {
                realPropertyAccounts = await _context.RealPropertyAccounts.Where(x => x.AcctNbr.StartsWith(AccountNumber)).AsNoTracking().ToListAsync();
            }

            if (!realPropertyAccounts.Any())
            {
                return NotFound();
            }

            return realPropertyAccounts;
        }

        [HttpGet("Search/{Query}")]
        public async Task<ActionResult<List<string>>> SearchForRealPropertyAccountNumbers(string Query)
        {
            if (string.IsNullOrWhiteSpace(Query))
            {
                return BadRequest();

            }

            Query = Query.Trim();

            if (IsDigitsOnly(Query) is false)
            {
                return BadRequest();
            }

            var realPropertyAccount = new List<string>();

            if (Query.Length <= 6)
            {
                realPropertyAccount = await _context.RealPropertyAccounts.Where(x => x.Major.StartsWith(Query)).Select(x => x.AcctNbr).Take(10).AsNoTracking().ToListAsync();
            }
            else if (Query.Length <= 10)
            {
                var major = Query.Substring(0, 6);
                var minor = Query.Substring(6, Query.Length - 6);
                realPropertyAccount = await _context.RealPropertyAccounts.Where(x => (x.Major == major) && (x.Minor.StartsWith(minor))).Select(x => x.AcctNbr).Take(10).AsNoTracking().ToListAsync();
            }
            else
            {
                realPropertyAccount = await _context.RealPropertyAccounts.Where(x => x.AcctNbr.StartsWith(Query)).Select(x => x.AcctNbr).Take(10).AsNoTracking().ToListAsync();
            }

            if (realPropertyAccount.Any() is false)
            {
                return NotFound();
            }

            return realPropertyAccount;
        }

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
