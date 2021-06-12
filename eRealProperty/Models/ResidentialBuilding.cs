using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class ResidentialBuilding
    {
        public Guid Id { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string ParcelNumber { get; set; }
        public int BldgNbr { get; set; }
        public int NbrLivingUnits { get; set; }
        public string Address { get; set; }
        public string BuildingNumber { get; set; }
        public string Fraction { get; set; }
        public string DirectionPrefix { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string DirectionSuffix { get; set; }
        public string ZipCode { get; set; }
        public string Stories { get; set; }
        public string BldgGrade { get; set; }
        public int BldgGradeVar { get; set; }
        public int SqFt1stFloor { get; set; }
        public int SqFtHalfFloor { get; set; }
        public int SqFt2ndFloor { get; set; }
        public int SqFtUpperFloor { get; set; }
        public int SqFtUnfinFull { get; set; }
        public int SqFtUnfinHalf { get; set; }
        public int SqFtTotLiving { get; set; }
        public int SqFtTotBasement { get; set; }
        public int SqFtFinBasement { get; set; }
        public string FinBasementGrade { get; set; }
        public int SqFtGarageBasement { get; set; }
        public int SqFtGarageAttached { get; set; }
        public string DaylightBasement { get; set; }
        public int SqFtOpenPorch { get; set; }
        public int SqFtEnclosedPorch { get; set; }
        public int SqFtDeck { get; set; }
        public string HeatSystem { get; set; }
        public string HeatSource { get; set; }
        public int BrickStone { get; set; }
        public string ViewUtilization { get; set; }
        public int Bedrooms { get; set; }
        public int BathHalfCount { get; set; }
        public int Bath3qtrCount { get; set; }
        public int BathFullCount { get; set; }
        public int FpSingleStory { get; set; }
        public int FpMultiStory { get; set; }
        public int FpFreestanding { get; set; }
        public int FpAdditional { get; set; }
        public int YrBuilt { get; set; }
        public int YrRenovated { get; set; }
        public int PcntComplete { get; set; }
        public int Obsolescence { get; set; }
        public int PcntNetCondition { get; set; }
        public string Condition { get; set; }
        public int AddnlCost { get; set; }
        public DateTime IngestedOn { get; set; }

        public static async Task<bool> IngestAsync(DbContextOptions<eRealPropertyContext> contextOptions)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_ResBldg.csv");

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
                int batchSize = Environment.ProcessorCount * 1000;
                csv.Read();
                csv.ReadHeader();
                var record = new ResidentialBuilding();

                while (csv.Read())
                {
                    record = csv.GetRecord<ResidentialBuilding>();
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

        public static async Task<bool> IngestByParcelNumberAsync(string parcelNumber, eRealPropertyContext context)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_ResBldg.csv");

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
                var record = new ResidentialBuilding();

                while (csv.Read())
                {
                    record = csv.GetRecord<ResidentialBuilding>();
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
            BldgGrade = GetBuildingGrade();
            FinBasementGrade = GetFinishedBasementGrade();
            HeatSystem = GetHeatSystem();
            HeatSource = GetHeatSource();
            Condition = GetCondition();
            return true;
        }

        public string GetBuildingGrade()
        {
            return BldgGrade switch
            {
                "0" => null,
                "1" => "1 Cabin",
                "2" => "2 Substandard",
                "3" => "3 Poor",
                "4" => "4 Low",
                "5" => "5 Fair",
                "6" => "6 Low Average",
                "7" => "7 Average",
                "8" => "8 Good",
                "9" => "9 Better",
                "10" => "10 Very Good",
                "11" => "11 Excellent",
                "12" => "12 Luxury",
                "13" => "13 Mansion",
                "20" => "Exceptional Properties",
                _ => string.Empty,
            };
        }

        public string GetFinishedBasementGrade()
        {
            return FinBasementGrade switch
            {
                "0" => null,
                "1" => "1 Cabin",
                "2" => "2 Substandard",
                "3" => "3 Poor",
                "4" => "4 Low",
                "5" => "5 Fair",
                "6" => "6 Low Average",
                "7" => "7 Average",
                "8" => "8 Good",
                "9" => "9 Better",
                "10" => "10 Very Good",
                "11" => "11 Excellent",
                "12" => "12 Luxury",
                "13" => "13 Mansion",
                "20" => "Exceptional Properties",
                _ => string.Empty,
            };
        }

        public string GetHeatSource()
        {
            return HeatSource switch
            {
                "0" => null,
                "1" => "Oil",
                "2" => "Gas",
                "3" => "Electricity",
                "4" => "Oil or Solar",
                "5" => "Gas or Solar",
                "6" => "Electricity or Solar",
                "7" => "Other",
                _ => string.Empty,
            };
        }

        public string GetCondition()
        {
            return Condition switch
            {
                "0" => null,
                "1" => "Poor",
                "2" => "Fair",
                "3" => "Average",
                "4" => "Good",
                "5" => "Very Good",
                _ => string.Empty,
            };
        }

        public string GetHeatSystem()
        {
            return HeatSystem switch
            {
                "0" => null,
                "1" => "Floor or Wall",
                "2" => "Gravity",
                "3" => "Radiant",
                "4" => "Electric Baseboard",
                "5" => "Forced Air",
                "6" => "Hot Water",
                "7" => "Heat Pump",
                "8" => "Other",
                _ => string.Empty,
            };
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
