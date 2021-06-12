using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class RealPropertyAccountTaxYear
    {
        [Key]
        public Guid Id { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string ParcelNumber { get; set; }
        public int TaxYr { get; set; }
        public int OmitYr { get; set; }
        public long ApprLandVal { get; set; }
        public long ApprImpsVal { get; set; }
        public long ApprImpIncr { get; set; }
        public long LandVal { get; set; }
        public long ImpsVal { get; set; }
        public string TaxValReason { get; set; }
        public string TaxStat { get; set; }
        public int LevyCode { get; set; }
        public string ChangeDate { get; set; }
        public string ChangeDocId { get; set; }
        public string Reason { get; set; }
        public char SplitCode { get; set; }
        public DateTime IngestedOn { get; set; }

        public static async Task<bool> IngestAsync(DbContextOptions<eRealPropertyContext> contextOptions)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_ValueHistory_V.csv");

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
                const int batchSize = 100;
                csv.Read();
                csv.ReadHeader();
                var record = new RealPropertyAccountTaxYear();

                while (csv.Read())
                {
                    record = csv.GetRecord<RealPropertyAccountTaxYear>();
                    record.Id = Guid.NewGuid();
                    record.IngestedOn = DateTime.Now;
                    var checkTranslation = record.TranslateFieldsUsingLookupsToText();
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

        public static async Task<bool> IngestByParcelNumberAsync(string parcelNumber, eRealPropertyContext context)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_ValueHistory_V.csv");

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
                var record = new RealPropertyAccountTaxYear();

                while (csv.Read())
                {
                    record = csv.GetRecord<RealPropertyAccountTaxYear>();
                    var major = parcelNumber.Substring(0, 6);
                    var minor = parcelNumber.Substring(6, 4);

                    if (record.Major == major && record.Minor == minor)
                    {
                        // Do something with the record.
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

        public string GetTaxableValueReason()
        {
            if (!string.IsNullOrWhiteSpace(TaxValReason))
            {
                return TaxValReason switch
                {
                    "CU" => "Current Use",
                    "DP" => "Destroyed Property",
                    "EX" => "Exempt from Taxes",
                    "FS" => "Senior Citizen Value",
                    "HI" => "Home Improvement",
                    "HP" => "Historic Property",
                    "MX" => "Multiple Reasons",
                    "NP" => "Non-Profit Org.",
                    "OP" => "Operating Property",
                    "UI" => "Undivided Interest",
                    _ => null,
                };
            }
            else
            {
                return null;
            }

        }

        // This is unused, because this column already has the values converted to English in the export.
        public string GetReason()
        {
            if (!string.IsNullOrWhiteSpace(Reason))
            {
                return Reason switch
                {
                    "3" => "None",
                    "4" => "Levy Code Change",
                    "5" => "Revalue",
                    "6" => "New Plat",
                    "7" => "Plat Kill",
                    "8" => "Tax Status Change",
                    "9" => "Reactivated",
                    "10" => "Kill by Cancel",
                    "11" => "Board Change",
                    "12" => "Segregation Or Merge",
                    "13" => "Kill by Merge",
                    "14" => "Omitted Assessment",
                    "15" => "Segregation",
                    "16" => "Merger",
                    "17" => "Legal/Abatement",
                    "18" => "Board of Equalization Change",
                    "19" => "Omit Correction",
                    "20" => "Condemnation",
                    "21" => "Legal Correction",
                    "22" => "Correction",
                    "23" => "Revalue - Timber",
                    "24" => "100% Value Law 73",
                    "25" => "Timber-Depl",
                    "26" => "Forest Land",
                    "27" => "Rec-value",
                    "28" => "Open Space",
                    "32" => "Segregation",
                    "33" => "Merger",
                    "34" => "Omitted Assessment",
                    "35" => "Segregation or Status Change",
                    "36" => "Merge or Status Change",
                    "37" => "Segregation or Code Change",
                    "38" => "Merge or Code Change",
                    "39" => "Omitted Assessment",
                    "40" => "Condemnation",
                    "41" => "Legal Change",
                    "42" => "Omitted Assessment",
                    "43" => "Omitted Revision",
                    "44" => "Correction",
                    "45" => "New Parcel",
                    "46" => "New Plat",
                    "47" => "Revalue",
                    "48" => "Maintenance",
                    "49" => "Revalue Factor",
                    "50" => "Home Improvement Exemption",
                    "51" => "Maintenance",
                    "52" => "June Board Order",
                    "53" => "July Board Order",
                    "54" => "November Board Order",
                    "55" => "State Board Order",
                    "56" => "Destroyed Property",
                    "57" => "Co-op Senior Citizen Exemption",
                    "58" => "Court Order",
                    "59" => "Revalue",
                    "60" => "Maintenance",
                    "61" => "Home Improvement Exemption",
                    "62" => "Amendment",
                    "63" => "Maintenance",
                    "64" => "Board Extension",
                    "65" => "Extension",
                    "67" => "Historic Property",
                    "68" => "Correction by Board",
                    "69" => "Senior Citizen Frozen Value",
                    "70" => "Senior Citizen Change",
                    _ => null,
                };
            }
            else
            {
                return null;
            }

        }

        public string GetTaxStatus()
        {
            if (!string.IsNullOrWhiteSpace(TaxStat))
            {
                return TaxStat switch
                {
                    "O" => "Operating",
                    "T" => "Taxable",
                    "X" => "Exempt",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }
    }

}
