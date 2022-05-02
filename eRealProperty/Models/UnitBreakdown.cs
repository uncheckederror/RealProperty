using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

using Flurl.Http;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class UnitBreakdown
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        [CsvHelper.Configuration.Attributes.Index(0)]
        public string Major { get; set; }
        [CsvHelper.Configuration.Attributes.Index(1)]
        public string Minor { get; set; }
        [Ignore]
        public string ParcelNumber { get; set; }
        [CsvHelper.Configuration.Attributes.Index(2)]
        public string UnitTypeItemId { get; set; }
        [CsvHelper.Configuration.Attributes.Index(3)]
        public int NbrThisType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public int SqFt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public string NbrBedrooms { get; set; }
        [CsvHelper.Configuration.Attributes.Index(6)]
        public string NbrBaths { get; set; }
        [Ignore]
        public DateTime IngestedOn { get; set; }

        public static async Task<bool> IngestAsync(eRealPropertyContext context, string pathToCSV, CsvConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(pathToCSV) || context is null || config is null)
            {
                return false;
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            using var reader = new StreamReader(pathToCSV, config.Encoding);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecordsAsync<UnitBreakdown>();


            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into UnitBreakdowns (Id, Major, Minor, ParcelNumber, UnitTypeItemId, NbrThisType, SqFt, NbrBedrooms, NbrBaths, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $UnitTypeItemId, $NbrThisType, $SqFt, $NbrBedrooms, $NbrBaths, $IngestedOn);";

            var Id = command.CreateParameter();
            Id.ParameterName = "$Id";
            command.Parameters.Add(Id);

            var Major = command.CreateParameter();
            Major.ParameterName = "$Major";
            command.Parameters.Add(Major);

            var Minor = command.CreateParameter();
            Minor.ParameterName = "$Minor";
            command.Parameters.Add(Minor);

            var ParcelNumber = command.CreateParameter();
            ParcelNumber.ParameterName = "$ParcelNumber";
            command.Parameters.Add(ParcelNumber);

            var UnitTypeItemId = command.CreateParameter();
            UnitTypeItemId.ParameterName = "$UnitTypeItemId";
            command.Parameters.Add(UnitTypeItemId);

            var NbrThisType = command.CreateParameter();
            NbrThisType.ParameterName = "$NbrThisType";
            command.Parameters.Add(NbrThisType);

            var SqFt = command.CreateParameter();
            SqFt.ParameterName = "$SqFt";
            command.Parameters.Add(SqFt);

            var NbrBedrooms = command.CreateParameter();
            NbrBedrooms.ParameterName = "$NbrBedrooms";
            command.Parameters.Add(NbrBedrooms);

            var NbrBaths = command.CreateParameter();
            NbrBaths.ParameterName = "$NbrBaths";
            command.Parameters.Add(NbrBaths);

            var IngestedOn = command.CreateParameter();
            IngestedOn.ParameterName = "$IngestedOn";
            command.Parameters.Add(IngestedOn);

            await foreach (var record in records)
            {
                record.Id = Guid.NewGuid();
                record.IngestedOn = DateTime.Now;
                record.TranslateFieldsUsingLookupsToText();

                Id.Value = record.Id;
                Major.Value = record.Major;
                Minor.Value = record.Minor;
                ParcelNumber.Value = record.ParcelNumber;
                UnitTypeItemId.Value = record.UnitTypeItemId;
                NbrThisType.Value = record.NbrThisType;
                SqFt.Value = record.SqFt;
                NbrBedrooms.Value = record.NbrBedrooms;
                NbrBaths.Value = record.NbrBaths;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            UnitTypeItemId = GetUnitTypeItemId();
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

        public string GetUnitTypeItemId()
        {
            return UnitTypeItemId switch
            {
                "1" => "Flat",
                "2" => "Townhouse",
                "3" => "Penthouse",
                "4" => "NursHme/Hospital:1-bd rms",
                "5" => "NursHme/Hospital:2-bd rms",
                "6" => "NursHme/Hospital:3-bd rms",
                "7" => "NursHme/Hospital:4-bd rms",
                "8" => "Rooming House",
                "9" => "Live/Work",
                _ => string.Empty,
            };
        }
    }
}
