using eRealProperty.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSide
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            // Get the configuration keys
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

            // Ready the ORM configuration
            var contextOptions = new DbContextOptionsBuilder<eRealPropertyContext>()
                    .UseSqlite(config.GetConnectionString("eRealPropertyContext"))
                    .Options;

            // Put the ORM to work and make sure we have a database
            using var db = new eRealPropertyContext(contextOptions);
            // Delete the existing database if it exists and then recreate it.
            // Handles senarios where data was only partially ingests or has expired.
            await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            var accounts = await RealPropertyAccount.IngestAsync(contextOptions);
            //var bldgs = await ResidentialBuilding.IngestAsync(contextOptions);
            //var legals = await LegalDiscription.IngestForAllExistingAccountsAsync(contextOptions);
            //await RealPropertyAccountSale.IngestAsync(contextOptions);
            var levys = await TaxLevy.IngestAsync(contextOptions);
            // These are disabled to speed up the development inner loop.
            // They should be reenabled when this app is deployed to production to improve performance.
            //var parcels = await PropertyParcel.IngestAsync(contextOptions);
            //var years = await RealPropertyAccountTaxYear.IngestAsync(contextOptions);
            //var legals = await LegalDiscription.IngestAsync(contextOptions);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
