﻿using CsvHelper;
using CsvHelper.Configuration;

using Flurl.Http;

using Microsoft.EntityFrameworkCore;

using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
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
                var transaction = context.Database.BeginTransaction();
                csv.Read();
                csv.ReadHeader();
                var record = new LegalDiscription();

                while (await csv.ReadAsync())
                {
                    record = csv.GetRecord<LegalDiscription>();
                    record.Id = Guid.NewGuid();
                    record.IngestedOn = DateTime.Now;
                    record.TranslateFieldsUsingLookupsToText();

                    var command = context.Database.GetDbConnection().CreateCommand();
                    command.CommandText =
                        $"insert into LegalDiscriptions (Id, Major, Minor, ParcelNumber, LegalDesc, IngestedOn) " +
                        $"values ($Id, $Major, $Minor, $ParcelNumber, $LegalDesc, $IngestedOn);";

                    var Id = command.CreateParameter();
                    Id.ParameterName = "$Id";
                    command.Parameters.Add(Id);
                    Id.Value = record.Id;

                    var Major = command.CreateParameter();
                    Major.ParameterName = "$Major";
                    command.Parameters.Add(Major);
                    Major.Value = record.Major;

                    var Minor = command.CreateParameter();
                    Minor.ParameterName = "$Minor";
                    command.Parameters.Add(Minor);
                    Minor.Value = record.Minor;

                    var ParcelNumber = command.CreateParameter();
                    ParcelNumber.ParameterName = "$ParcelNumber";
                    command.Parameters.Add(ParcelNumber);
                    ParcelNumber.Value = record.ParcelNumber;

                    var LegalDesc = command.CreateParameter();
                    LegalDesc.ParameterName = "$LegalDesc";
                    command.Parameters.Add(LegalDesc);
                    LegalDesc.Value = record.LegalDesc;

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

        public static async Task<bool> IngestForAllExistingAccountsAsync(eRealPropertyContext context, string zipUrl, string fileName)
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
