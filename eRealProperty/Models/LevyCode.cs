using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class TaxLevy
    {
        [Key]
        public Guid Id { get; set; }
        public string DistrictAbbrev { get; set; }
        public string LevyCode { get; set; }
        public string DistrictName { get; set; }
        public DateTime IngestedOn { get; set; }

        public static async Task<bool> IngestAsync(DbContextOptions<eRealPropertyContext> contextOptions)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_LevyDistXRef.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(pathToCSV))
            using (var csv = new CsvReader(reader, config))
            {
                using var context = new eRealPropertyContext(contextOptions);
                context.ChangeTracker.AutoDetectChangesEnabled = false;

                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var record = csv.GetRecord<TaxLevy>();
                    record.Id = Guid.NewGuid();
                    record.IngestedOn = DateTime.Now;
                    context.Add(record);
                }

                await context.SaveChangesAsync();
            }
            return true;
        }
    }
}
