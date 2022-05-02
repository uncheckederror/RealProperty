using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class TaxLevy
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        [CsvHelper.Configuration.Attributes.Index(0)]
        public string DistrictAbbrev { get; set; }
        [CsvHelper.Configuration.Attributes.Index(1)]
        public string LevyCode { get; set; }
        [CsvHelper.Configuration.Attributes.Index(2)]
        public string DistrictName { get; set; }
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

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into LevyCodes (Id, DistrictAbbrev, LevyCode, DistrictName, IngestedOn) " +
                $"values ($Id, $DistrictAbbrev, $LevyCode, $DistrictName, $IngestedOn);";

            var Id = command.CreateParameter();
            Id.ParameterName = "$Id";
            command.Parameters.Add(Id);

            var DistrictAbbrev = command.CreateParameter();
            DistrictAbbrev.ParameterName = "$DistrictAbbrev";
            command.Parameters.Add(DistrictAbbrev);

            var LevyCode = command.CreateParameter();
            LevyCode.ParameterName = "$LevyCode";
            command.Parameters.Add(LevyCode);

            var DistrictName = command.CreateParameter();
            DistrictName.ParameterName = "$DistrictName";
            command.Parameters.Add(DistrictName);

            var IngestedOn = command.CreateParameter();
            IngestedOn.ParameterName = "$IngestedOn";
            command.Parameters.Add(IngestedOn);

            var records = csv.GetRecordsAsync<TaxLevy>();

            await foreach (var record in records)
            {
                record.Id = Guid.NewGuid();
                record.IngestedOn = DateTime.Now;

                Id.Value = record.Id;
                DistrictAbbrev.Value = record.DistrictAbbrev;
                LevyCode.Value = record.LevyCode;
                DistrictName.Value = record.DistrictName;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }
    }
}
