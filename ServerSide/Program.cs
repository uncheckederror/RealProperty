using CsvHelper.Configuration;

using eRealProperty.Models;

using Flurl.Http;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Events;

using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
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

            static async Task<string> GetFilePathAsync(string zipUrl, string fileName)
            {
                if (string.IsNullOrWhiteSpace(zipUrl) || string.IsNullOrWhiteSpace(fileName))
                {
                    return string.Empty;
                }

                Log.Information($"Downloading {zipUrl}...");
                var pathtoFile = await zipUrl.DownloadFileAsync(AppContext.BaseDirectory);
                var pathToCSV = Path.Combine(AppContext.BaseDirectory, fileName);

                var fileTypes = new string[] { ".txt", ".csv" };
                // If a file with the same name already exists it will break the downloading process, so we need to make sure they are deleted.
                foreach (var type in fileTypes)
                {
                    var filePath = Path.Combine(AppContext.BaseDirectory, Path.GetFileNameWithoutExtension(pathtoFile) + type);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                Log.Information($"Unzipping {pathToCSV}...");
                if (!File.Exists(pathToCSV))
                {
                    ZipFile.ExtractToDirectory(pathtoFile, AppContext.BaseDirectory);
                }

                File.Delete(pathtoFile);

                return pathToCSV;
            }

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                Delimiter = ",",
                BadDataFound = null,
                MissingFieldFound = null,
                CacheFields = true,
                Encoding = System.Text.Encoding.ASCII
            };

            // Load the data if there's nothing in the database.
            if (!await db.RealPropertyAccounts.AnyAsync())
            {
                Log.Information("Ingesting Real Accounts.");
                await db.Database.ExecuteSqlRawAsync("DELETE FROM RealPropertyAccounts;");
                var path = await GetFilePathAsync(config["DataSources:RealPropertyAccount"], config["DataSources:RealPropertyAccountFileName"]);
                var accounts = await RealPropertyAccount.IngestAsync(db, path, csvConfig);
                if (accounts)
                {
                    Log.Information($"Ingested {await db.RealPropertyAccounts.CountAsync()} Real Accounts.");
                }
                else
                {
                    Log.Fatal("Failed to ingest Real Accounts.");
                }
                File.Delete(path);
            }

            //if (!await db.ResidentialBuildings.AnyAsync())
            //{
            //    Log.Information("Ingesting Residential Buildings.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM ResidentialBuildings;");
            //    var path = await GetFilePathAsync(config["DataSources:ResidentialBuilding"], config["DataSources:ResidentialBuildingFileName"]);
            //    var bldgs = await ResidentialBuilding.IngestAsync(db, path, csvConfig);
            //    if (bldgs)
            //    {
            //        Log.Information($"Ingested {await db.ResidentialBuildings.CountAsync()} Residential Buildings.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Residential Buildings.");
            //    }
            //    File.Delete(path);
            //}

            //if (!await db.CommericalBuildings.AnyAsync())
            //{
            //    Log.Information("Ingesting Commerical Buildings.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM CommericalBuildings;");
            //    var path = await GetFilePathAsync(config["DataSources:CommericalBuilding"], config["DataSources:CommericalBuildingFileName"]);
            //    var years = await CommericalBuilding.IngestAsync(db, path, csvConfig);
            //    if (years)
            //    {
            //        Log.Information($"Ingested {await db.CommericalBuildings.CountAsync()} Commerical Buildings.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Commerical Buildings.");
            //    }
            //    File.Delete(path);

            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM CommericalBuildingFeatures;");
            //    path = await GetFilePathAsync(config["DataSources:CommericalBuilding"], config["DataSources:CommericalBuildingFeatureFileName"]);
            //    years = await CommericalBuildingFeature.IngestAsync(db, path, csvConfig);
            //    if (years)
            //    {
            //        Log.Information($"Ingested {await db.CommericalBuildingFeatures.CountAsync()} Commerical Building Features.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Commerical Building Features.");
            //    }
            //    File.Delete(path);

            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM CommericalBuildingSections;");
            //    path = await GetFilePathAsync(config["DataSources:CommericalBuilding"], config["DataSources:CommericalBuildingSectionFileName"]);
            //    years = await CommericalBuildingSection.IngestAsync(db, path, csvConfig);
            //    if (years)
            //    {
            //        Log.Information($"Ingested {await db.CommericalBuildingSections.CountAsync()} Commerical Building Sections.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Commerical Building Sections.");
            //    }
            //    File.Delete(path);
            //}

            //if (!await db.CondoComplexes.AnyAsync())
            //{
            //    Log.Information("Ingesting Condo Complexes.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM CondoComplexes;");
            //    var path = await GetFilePathAsync(config["DataSources:Condos"], config["DataSources:CondoComplexFileName"]);
            //    var bldgs = await CondoComplex.IngestAsync(db, path, csvConfig);
            //    if (bldgs)
            //    {
            //        Log.Information($"Ingested {await db.CondoComplexes.CountAsync()} Condo Complexes.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Condo Complexes.");
            //    }
            //    File.Delete(path);

            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM CondoUnits;");
            //    path = await GetFilePathAsync(config["DataSources:Condos"], config["DataSources:CondoUnitFileName"]);
            //    bldgs = await CondoUnit.IngestAsync(db, path, csvConfig);
            //    if (bldgs)
            //    {
            //        Log.Information($"Ingested {await db.CondoUnits.CountAsync()} Condo Units.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Condo Units.");
            //    }
            //    File.Delete(path);
            //}

            //if (!await db.ApartmentComplexes.AnyAsync())
            //{
            //    Log.Information("Ingesting Apartment Complexes.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM ApartmentComplexes;");
            //    var path = await GetFilePathAsync(config["DataSources:ApartmentComplexes"], config["DataSources:ApartmentComplexesFileName"]);
            //    var bldgs = await ApartmentComplex.IngestAsync(db, path, csvConfig);
            //    if (bldgs)
            //    {
            //        Log.Information($"Ingested {await db.ApartmentComplexes.CountAsync()} Apartment Complexes.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Apartment Complexes.");
            //    }
            //    File.Delete(path);

            //    Log.Information("Ingesting Unit Breakdowns.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM UnitBreakdowns;");
            //    path = await GetFilePathAsync(config["DataSources:UnitBreakdowns"], config["DataSources:UnitBreakdownsFileName"]);
            //    bldgs = await UnitBreakdown.IngestAsync(db, path, csvConfig);
            //    if (bldgs)
            //    {
            //        Log.Information($"Ingested {await db.UnitBreakdowns.CountAsync()} Unit Breakdowns.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Unit Breakdowns.");
            //    }
            //    File.Delete(path);
            //}

            //if (!await db.LegalDiscriptions.AnyAsync())
            //{
            //    Log.Information("Ingesting Legal Descriptions.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM LegalDiscriptions;");
            //    var path = await GetFilePathAsync(config["DataSources:LegalDiscription"], config["DataSources:LegalDiscriptionFileName"]);
            //    var legals = await LegalDiscription.IngestAsync(db, path, csvConfig);
            //    if (legals)
            //    {
            //        Log.Information($"Ingested {await db.LegalDiscriptions.CountAsync()} Legal Descriptions.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Legal Descriptions.");
            //    }
            //    File.Delete(path);
            //}

            //if (!await db.Sales.AnyAsync())
            //{
            //    Log.Information("Ingesting Sales.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM Sales;");
            //    var path = await GetFilePathAsync(config["DataSources:RealPropertyAccountSale"], config["DataSources:RealPropertyAccountSaleFileName"]);
            //    var sales = await RealPropertyAccountSale.IngestAsync(db, path, csvConfig);
            //    if (sales)
            //    {
            //        Log.Information($"Ingested {await db.Sales.CountAsync()} Sales.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Sales.");
            //    }
            //    File.Delete(path);
            //}

            //if (!await db.LevyCodes.AnyAsync())
            //{
            //    Log.Information("Ingesting Levy Codes.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM LevyCodes;");
            //    var path = await GetFilePathAsync(config["DataSources:TaxLevy"], config["DataSources:TaxLevyFileName"]);
            //    var levys = await TaxLevy.IngestAsync(db, path, csvConfig);
            //    if (levys)
            //    {
            //        Log.Information($"Ingested {await db.LevyCodes.CountAsync()} Levy Codes.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Levy Codes.");
            //    }
            //    File.Delete(path);
            //}

            //if (!await db.PropertyParcels.AnyAsync())
            //{
            //    Log.Information("Ingesting Property Parcels.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM PropertyParcels;");
            //    var path = await GetFilePathAsync(config["DataSources:PropertyParcel"], config["DataSources:PropertyParcelFileName"]);
            //    var parcels = await PropertyParcel.IngestAsync(db, path, csvConfig);
            //    if (parcels)
            //    {
            //        Log.Information($"Ingested {await db.PropertyParcels.CountAsync()} Property Parcels.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Property Parcels.");
            //    }
            //    File.Delete(path);
            //}

            //if (!await db.RealPropertyAccountTaxYears.AnyAsync())
            //{
            //    Log.Information($"Ingesting Tax Years {await db.RealPropertyAccountTaxYears.CountAsync()} found.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM RealPropertyAccountTaxYears;");
            //    var path = await GetFilePathAsync(config["DataSources:RealPropertyAccountTaxYear"], config["DataSources:RealPropertyAccountTaxYearFileName"]);
            //    var years = await RealPropertyAccountTaxYear.IngestAsync(db, path, csvConfig);
            //    if (years)
            //    {
            //        Log.Information($"Ingested {await db.RealPropertyAccountTaxYears.CountAsync()} Tax Years.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Tax Years.");
            //    }
            //    File.Delete(path);
            //}

            //if (!await db.Reviews.AnyAsync())
            //{
            //    Log.Information("Ingesting Reviews.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM Reviews;");
            //    var path = await GetFilePathAsync(config["DataSources:ReviewHistory"], config["DataSources:ReviewHistoryFileName"]);
            //    var years = await Review.IngestAsync(db, path, csvConfig);
            //    if (years)
            //    {
            //        Log.Information($"Ingested {await db.Reviews.CountAsync()} Reviews.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Reviews.");
            //    }
            //    File.Delete(path);

            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM ReviewDescriptions;");
            //    path = await GetFilePathAsync(config["DataSources:ReviewHistory"], config["DataSources:ReviewDescriptionFileName"]);
            //    years = await ReviewDescription.IngestAsync(db, path, csvConfig);
            //    if (years)
            //    {
            //        Log.Information($"Ingested {await db.ReviewDescriptions.CountAsync()} Review Descriptions.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Review Descriptions.");
            //    }
            //    File.Delete(path);

            //}

            //if (!await db.Permits.AnyAsync())
            //{
            //    Log.Information("Ingesting Permits.");
            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM Permits;");
            //    var path = await GetFilePathAsync(config["DataSources:Permit"], config["DataSources:PermitFileName"]);
            //    var years = await Permit.IngestAsync(db, path, csvConfig);
            //    if (years)
            //    {
            //        Log.Information($"Ingested {await db.Permits.CountAsync()} Permits.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Permits.");
            //    }
            //    File.Delete(path);

            //    await db.Database.ExecuteSqlRawAsync("DELETE FROM PermitDetailHistories;");
            //    path = await GetFilePathAsync(config["DataSources:ReviewHistory"], config["DataSources:PermitDetailHistoryFileName"]);
            //    years = await PermitDetailHistory.IngestAsync(db, path, csvConfig);
            //    if (years)
            //    {
            //        Log.Information($"Ingested {await db.PermitDetailHistories.CountAsync()} Permit Details.");
            //    }
            //    else
            //    {
            //        Log.Fatal("Failed to ingest Permit Details.");
            //    }
            //    File.Delete(path);
            //}

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
