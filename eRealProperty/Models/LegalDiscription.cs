using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

using Microsoft.EntityFrameworkCore;

using Serilog;

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class LegalDiscription
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
        public string LegalDesc { get; set; }
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

            var records = csv.GetRecordsAsync<LegalDiscription>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into LegalDiscriptions (Id, Major, Minor, ParcelNumber, LegalDesc, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $LegalDesc, $IngestedOn);";

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

            var LegalDesc = command.CreateParameter();
            LegalDesc.ParameterName = "$LegalDesc";
            command.Parameters.Add(LegalDesc);

            var IngestedOn = command.CreateParameter();
            IngestedOn.ParameterName = "$IngestedOn";
            command.Parameters.Add(IngestedOn);

            var count = 0;

            await foreach (var record in records)
            {
                record.Id = Guid.NewGuid();
                record.IngestedOn = DateTime.Now;
                record.TranslateFieldsUsingLookupsToText();

                Id.Value = record.Id;
                Major.Value = record.Major;
                Minor.Value = record.Minor;
                ParcelNumber.Value = record.ParcelNumber;
                LegalDesc.Value = record.LegalDesc;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
                count++;

                if (count % 10000 == 0)
                {
                    Log.Information($"Ingested {count} Real Property Accounts.");
                }
            }

            await transaction.CommitAsync();
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
