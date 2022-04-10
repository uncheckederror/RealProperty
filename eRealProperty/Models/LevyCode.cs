using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

using Flurl.Http;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.IO.Compression;
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

        public static async Task<bool> IngestAsync(eRealPropertyContext context, string zipUrl, string fileName)
        {
            if (string.IsNullOrWhiteSpace(zipUrl) || string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

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

            if (!File.Exists(pathToCSV))
            {
                ZipFile.ExtractToDirectory(pathtoFile, AppContext.BaseDirectory);
            }

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

                using var transaction = await context.Database.BeginTransactionAsync();

                while (csv.Read())
                {
                    var record = csv.GetRecord<TaxLevy>();
                    record.Id = Guid.NewGuid();
                    record.IngestedOn = DateTime.Now;

                    var command = context.Database.GetDbConnection().CreateCommand();
                    command.CommandText =
                        $"insert into LevyCodes (Id, DistrictAbbrev, LevyCode, DistrictName, IngestedOn) " +
                        $"values ($Id, $DistrictAbbrev, $LevyCode, $DistrictName, $IngestedOn);";

                    var Id = command.CreateParameter();
                    Id.ParameterName = "$Id";
                    command.Parameters.Add(Id);
                    Id.Value = record.Id;

                    var DistrictAbbrev = command.CreateParameter();
                    DistrictAbbrev.ParameterName = "$DistrictAbbrev";
                    command.Parameters.Add(DistrictAbbrev);
                    DistrictAbbrev.Value = record.DistrictAbbrev;

                    var LevyCode = command.CreateParameter();
                    LevyCode.ParameterName = "$LevyCode";
                    command.Parameters.Add(LevyCode);
                    LevyCode.Value = record.LevyCode;

                    var DistrictName = command.CreateParameter();
                    DistrictName.ParameterName = "$DistrictName";
                    command.Parameters.Add(DistrictName);
                    DistrictName.Value = record.DistrictName;

                    var IngestedOn = command.CreateParameter();
                    IngestedOn.ParameterName = "$IngestedOn";
                    command.Parameters.Add(IngestedOn);
                    IngestedOn.Value = record.IngestedOn;

                    await command.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            return true;
        }
    }
}
