using eRealProperty.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

using System;
using System.Threading.Tasks;

namespace ServerSide
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

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
            await db.Database.MigrateAsync();

            // Load the data if there's nothing in the database.
            if (!await db.RealPropertyAccounts.AnyAsync())
            {
                Log.Information("Ingesting Real Accounts");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM RealPropertyAccounts;");
                var accounts = await RealPropertyAccount.IngestAsync(db, config["DataSources:RealPropertyAccount"], config["DataSources:RealPropertyAccountFileName"]);
                if (accounts)
                {
                    Log.Information($"Ingested {await db.RealPropertyAccounts.CountAsync()} Real Accounts.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Real Accounts.");
                }
            }

            if (!await db.ResidentialBuildings.AnyAsync())
            {
                Log.Information("Ingesting Residential Buildings.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM ResidentialBuildings;");
                var bldgs = await ResidentialBuilding.IngestAsync(db, config["DataSources:ResidentialBuilding"], config["DataSources:ResidentialBuildingFileName"]);
                if (bldgs)
                {
                    Log.Information($"Ingested {await db.RealPropertyAccounts.CountAsync()} Residential Buildings.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Residential Buildings.");
                }
            }

            if (!await db.CommericalBuildings.AnyAsync())
            {
                Log.Information("Ingesting Commerical Buildings.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM CommericalBuildings;");
                var years = await CommericalBuilding.IngestAsync(db, config["DataSources:CommericalBuilding"], config["DataSources:CommericalBuildingFileName"]);
                if (years)
                {
                    Log.Information($"Ingested {await db.CommericalBuildings.CountAsync()} Commerical Buildings.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Commerical Buildings.");
                }


                await db.Database.ExecuteSqlRawAsync("DELETE FROM CommericalBuildingFeatures;");
                years = await CommericalBuildingFeature.IngestAsync(db, config["DataSources:CommericalBuilding"], config["DataSources:CommericalBuildingFeatureFileName"]);
                if (years)
                {
                    Log.Information($"Ingested {await db.PermitDetailHistories.CountAsync()} Commerical Building Features.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Commerical Building Features.");
                }

                await db.Database.ExecuteSqlRawAsync("DELETE FROM CommericalBuildingSections;");
                years = await CommericalBuildingSection.IngestAsync(db, config["DataSources:CommericalBuilding"], config["DataSources:CommericalBuildingSectionFileName"]);
                if (years)
                {
                    Log.Information($"Ingested {await db.PermitDetailHistories.CountAsync()} Commerical Building Sections.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Commerical Building Sections.");
                }
            }

            if (!await db.CondoComplexes.AnyAsync())
            {
                Log.Information("Ingesting Condo Complexes.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM CondoComplexes;");
                var bldgs = await CondoComplex.IngestAsync(db, config["DataSources:Condos"], config["DataSources:CondoComplexFileName"]);
                if (bldgs)
                {
                    Log.Information($"Ingested {await db.CondoComplexes.CountAsync()} Condo Complexes.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Condo Complexes.");
                }

                await db.Database.ExecuteSqlRawAsync("DELETE FROM CondoUnits;");
                bldgs = await CondoUnit.IngestAsync(db, config["DataSources:Condos"], config["DataSources:CondoUnitFileName"]);
                if (bldgs)
                {
                    Log.Information($"Ingested {await db.CondoComplexes.CountAsync()} Condo Units.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Condo Units.");
                }
            }

            if (!await db.LegalDiscriptions.AnyAsync())
            {
                Log.Information("Ingesting Legal Descriptions.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM LegalDiscriptions;");
                var legals = await LegalDiscription.IngestAsync(db, config["DataSources:LegalDiscription"], config["DataSources:LegalDiscriptionFileName"]);
                if (legals)
                {
                    Log.Information($"Ingested {await db.RealPropertyAccounts.CountAsync()} Legal Descriptions.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Legal Descriptions.");
                }
            }

            if (!await db.Sales.AnyAsync())
            {
                Log.Information("Ingesting Sales.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM Sales;");
                var sales = await RealPropertyAccountSale.IngestAsync(db, config["DataSources:RealPropertyAccountSale"], config["DataSources:RealPropertyAccountSaleFileName"]);
                if (sales)
                {
                    Log.Information($"Ingested {await db.RealPropertyAccounts.CountAsync()} Sales.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Sales.");
                }
            }

            if (!await db.LevyCodes.AnyAsync())
            {
                Log.Information("Ingesting Levy Codes.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM LevyCodes;");
                var levys = await TaxLevy.IngestAsync(db, config["DataSources:TaxLevy"], config["DataSources:TaxLevyFileName"]);
                if (levys)
                {
                    Log.Information($"Ingested {await db.RealPropertyAccounts.CountAsync()} Levy Codes.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Levy Codes.");
                }
            }

            if (!await db.PropertyParcels.AnyAsync())
            {
                Log.Information("Ingesting Property Parcels.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM PropertyParcels;");
                var parcels = await PropertyParcel.IngestAsync(db, config["DataSources:PropertyParcel"], config["DataSources:PropertyParcelFileName"]);
                if (parcels)
                {
                    Log.Information($"Ingested {await db.RealPropertyAccounts.CountAsync()} Property Parcels.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Property Parcels.");
                }
            }

            if (!await db.RealPropertyAccountTaxYears.AnyAsync())
            {
                Log.Information("Ingesting Tax Years.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM RealPropertyAccountTaxYears;");
                var years = await RealPropertyAccountTaxYear.IngestAsync(db, config["DataSources:RealPropertyAccountTaxYear"], config["DataSources:RealPropertyAccountTaxYearFileName"]);
                if (years)
                {
                    Log.Information($"Ingested {await db.RealPropertyAccountTaxYears.CountAsync()} Tax Years.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Tax Years.");
                }
            }

            if (!await db.Reviews.AnyAsync())
            {
                Log.Information("Ingesting Reviews.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM Reviews;");
                var years = await Review.IngestAsync(db, config["DataSources:ReviewHistory"], config["DataSources:ReviewHistoryFileName"]);
                if (years)
                {
                    Log.Information($"Ingested {await db.Reviews.CountAsync()} Reviews.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Reviews.");
                }

                await db.Database.ExecuteSqlRawAsync("DELETE FROM ReviewDescriptions;");
                years = await ReviewDescription.IngestAsync(db, config["DataSources:ReviewHistory"], config["DataSources:ReviewDescriptionFileName"]);
                if (years)
                {
                    Log.Information($"Ingested {await db.ReviewDescriptions.CountAsync()} Review Descriptions.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Review Descriptions.");
                }
            }

            if (!await db.Permits.AnyAsync())
            {
                Log.Information("Ingesting Permits.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM Permits;");
                var years = await Permit.IngestAsync(db, config["DataSources:Permit"], config["DataSources:PermitFileName"]);
                if (years)
                {
                    Log.Information($"Ingested {await db.Permits.CountAsync()} Permits.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Permits.");
                }


                await db.Database.ExecuteSqlRawAsync("DELETE FROM PermitDetailHistories;");
                years = await PermitDetailHistory.IngestAsync(db, config["DataSources:ReviewHistory"], config["DataSources:PermitDetailHistoryFileName"]);
                if (years)
                {
                    Log.Information($"Ingested {await db.PermitDetailHistories.CountAsync()} Permit Details.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Permit Details.");
                }
            }
            
            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
