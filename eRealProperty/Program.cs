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

namespace eRealProperty
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            //await db.Database.EnsureDeletedAsync();
            await db.Database.EnsureCreatedAsync();

            //var parcels = await PropertyParcel.IngestAsync(db);
            //var accounts = await RealPropertyAccount.IngestAsync(db);
            //var years = await RealPropertyAccountTaxYear.IngestAsync(db);

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
