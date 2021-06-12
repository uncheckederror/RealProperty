using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.EntityFrameworkCore;

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class LegalDiscription
    {
        public Guid Id { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string ParcelNumber { get; set; }
        public string LegalDesc { get; set; }
        public DateTime IngestedOn { get; set; }

        public static async Task<bool> IngestAsync(DbContextOptions<eRealPropertyContext> contextOptions)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_Legal.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(pathToCSV))
            using (var csv = new CsvReader(reader, config))
            {
                var context = new eRealPropertyContext(contextOptions);
                context.ChangeTracker.AutoDetectChangesEnabled = false;

                var count = 0;
                const int batchSize = 10000;
                csv.Read();
                csv.ReadHeader();
                var record = new LegalDiscription();

                while (csv.Read())
                {
                    record = csv.GetRecord<LegalDiscription>();
                    record.Id = Guid.NewGuid();
                    record.IngestedOn = DateTime.Now;
                    record.TranslateFieldsUsingLookupsToText();
                    count++;
                    await context.AddAsync(record);

                    if (count != 0 && count % batchSize == 0)
                    {
                        await context.SaveChangesAsync();
                        context = new eRealPropertyContext(contextOptions);
                        context.ChangeTracker.AutoDetectChangesEnabled = false;
                    }
                }
                await context.SaveChangesAsync();
            }
            return true;
        }

        public static async Task<bool> IngestForAllExistingAccountsAsync(DbContextOptions<eRealPropertyContext> contextOptions)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_Legal.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(pathToCSV))
            using (var csv = new CsvReader(reader, config))
            {
                var context = new eRealPropertyContext(contextOptions);

                csv.Read();
                csv.ReadHeader();
                var record = new LegalDiscription();

                while (csv.Read())
                {
                    record = csv.GetRecord<LegalDiscription>();
                    record.TranslateFieldsUsingLookupsToText();
                    var realAccounts = await context.RealPropertyAccounts.Where(x => x.ParcelNumber == record.ParcelNumber).ToListAsync();
                    foreach (var account in realAccounts)
                    {
                        account.LegalDescription = record.LegalDesc;
                    }
                }
                await context.SaveChangesAsync();
            }
            return true;
        }

        public static async Task<bool> IngestByParcelNumberAsync(string parcelNumber, eRealPropertyContext context)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_Legal.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(pathToCSV))
            using (var csv = new CsvReader(reader, config))
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;

                csv.Read();
                csv.ReadHeader();
                var record = new LegalDiscription();

                while (csv.Read())
                {
                    record = csv.GetRecord<LegalDiscription>();

                    if ($"{record.Major}{record.Minor}" == parcelNumber)
                    {
                        record.Id = Guid.NewGuid();
                        record.IngestedOn = DateTime.Now;
                        var checkTranslation = record.TranslateFieldsUsingLookupsToText();
                        await context.AddAsync(record);
                    }
                }
                await context.SaveChangesAsync();
            }
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();

            return true;
        }

        public string GetParcelNumber()
        {
            if (!string.IsNullOrWhiteSpace(Major)
                && !string.IsNullOrWhiteSpace(Minor)
                && Major.Length == 6
                && Minor.Length == 4)
            {
                return $"{Major}{Minor}";
            }
            else
            {
                return null;
            }
        }
    }
}
