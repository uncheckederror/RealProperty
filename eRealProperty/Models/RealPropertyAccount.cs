using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class RealPropertyAccount
    {
        [Key]
        public Guid Id { get; set; }
        public string AcctNbr { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string ParcelNumber { get; set; }
        public string AttnLine { get; set; }
        public string AddrLine { get; set; }
        public string CityState { get; set; }
        public string ZipCode { get; set; }
        public string LevyCode { get; set; }
        public string TaxStat { get; set; }
        public int BillYr { get; set; }
        public string NewConstructionFlag { get; set; }
        public string TaxValReason { get; set; }
        public string ApprLandVal { get; set; }
        public string ApprImpsVal { get; set; }
        public string TaxableLandVal { get; set; }
        public string TaxableImpsVal { get; set; }
        public DateTime IngestedOn { get; set; }
        // From the legal description file.
        public string LegalDescription { get; set; }

        public static async Task<bool> IngestAsync(DbContextOptions<eRealPropertyContext> contextOptions)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_RPAcct_NoName.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(pathToCSV))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();

                var records = new List<RealPropertyAccount>();

                while (csv.Read())
                {
                    var record = new RealPropertyAccount
                    {
                        Id = Guid.NewGuid(),
                        AcctNbr = csv.GetField<string>("AcctNbr"),
                        AddrLine = csv.GetField<string>("AddrLine"),
                        ApprImpsVal = csv.GetField<string>("ApprImpsVal"),
                        ApprLandVal = csv.GetField<string>("ApprLandVal"),
                        AttnLine = csv.GetField<string>("AttnLine"),
                        BillYr = csv.GetField<int>("BillYr"),
                        CityState = csv.GetField<string>("CityState"),
                        TaxStat = csv.GetField<string>("TaxStat"),
                        LevyCode = csv.GetField<string>("LevyCode"),
                        Major = csv.GetField<string>("Major"),
                        Minor = csv.GetField<string>("Minor"),
                        NewConstructionFlag = csv.GetField<string>("NewConstructionFlag"),
                        TaxableImpsVal = csv.GetField<string>("TaxableImpsVal"),
                        TaxableLandVal = csv.GetField<string>("TaxableLandVal"),
                        TaxValReason = csv.GetField<string>("TaxValReason"),
                        ZipCode = csv.GetField<string>("ZipCode"),
                        IngestedOn = DateTime.Now
                    };

                    record.TranslateFieldsUsingLookupsToText();
                    records.Add(record);

                    if (records.Count == Environment.ProcessorCount * 10000)
                    {
                        using var context = new eRealPropertyContext(contextOptions);
                        context.ChangeTracker.AutoDetectChangesEnabled = false;
                        context.AddRange(records);
                        await context.SaveChangesAsync();
                        records = new List<RealPropertyAccount>();
                    }
                }

                if (records.Count > 0)
                {
                    using var context = new eRealPropertyContext(contextOptions);
                    context.ChangeTracker.AutoDetectChangesEnabled = false;
                    context.AddRange(records);
                    await context.SaveChangesAsync();
                }
            }

            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            TaxStat = GetTaxStatus();
            TaxValReason = GetTaxableValueReason();

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

        public string GetTaxStatus()
        {
            if (!string.IsNullOrEmpty(TaxStat))
            {
                return TaxStat switch
                {
                    "T" => "Taxable",
                    "X" => "Exempt",
                    "O" => "Operating",
                    _ => null
                };
            }
            else
            {
                return null;
            }
        }

        public string GetTaxableValueReason()
        {
            if (!string.IsNullOrEmpty(TaxValReason))
            {
                return TaxValReason switch
                {
                    "FS" => "Senior Citizen Exemption",
                    "EX" => "Exempt",
                    "OP" => "Operating",
                    "NP" => "Non-profit Exemption",
                    "CU" => "Open Space Exemption",
                    "HI" => "Home Improvement Exemption",
                    "HP" => "Historic Property Exemption",
                    "MX" => "More than one Exemption reason applies",
                    _ => null
                };
            }
            else
            {
                return null;
            }
        }
    }
}
