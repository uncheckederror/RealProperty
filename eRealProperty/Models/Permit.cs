using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

using Flurl.Http;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class Permit
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
        public string PermitNbr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(3)]
        public string PermitType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public DateTime IssueDate { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public int PermitVal { get; set; }
        [CsvHelper.Configuration.Attributes.Index(6)]
        public string PermitStatus { get; set; }
        [CsvHelper.Configuration.Attributes.Index(7)]
        public string PcntComplete { get; set; }
        [CsvHelper.Configuration.Attributes.Index(8)]
        public string UpdatedBy { get; set; }
        [CsvHelper.Configuration.Attributes.Index(9)]
        public DateTime UpdateDate { get; set; }
        [Ignore]
        public DateTime IngestedOn { get; set; }
        [Ignore]
        [NotMapped]
        public string ProjectName { get; set; }

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
                Delimiter = ",",
                MissingFieldFound = null,
                BadDataFound = null,
                CacheFields = true,
                Encoding = System.Text.Encoding.ASCII,
                TrimOptions = TrimOptions.InsideQuotes
            };

            using var transaction = await context.Database.BeginTransactionAsync();
            using var reader = new StreamReader(pathToCSV, System.Text.Encoding.ASCII);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecordsAsync<Permit>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into Permits (Id, Major, Minor, ParcelNumber, PermitNbr, PermitType, IssueDate, PermitVal, PermitStatus, PcntComplete, UpdatedBy, UpdateDate, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $PermitNbr, $PermitType, $IssueDate, $PermitVal, $PermitStatus, $PcntComplete, $UpdatedBy, $UpdateDate, $IngestedOn);";

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

            var PermitNbr = command.CreateParameter();
            PermitNbr.ParameterName = "$PermitNbr";
            command.Parameters.Add(PermitNbr);

            var PermitType = command.CreateParameter();
            PermitType.ParameterName = "$PermitType";
            command.Parameters.Add(PermitType);

            var IssueDate = command.CreateParameter();
            IssueDate.ParameterName = "$IssueDate";
            command.Parameters.Add(IssueDate);

            var PermitVal = command.CreateParameter();
            PermitVal.ParameterName = "$PermitVal";
            command.Parameters.Add(PermitVal);

            var PermitStatus = command.CreateParameter();
            PermitStatus.ParameterName = "$PermitStatus";
            command.Parameters.Add(PermitStatus);

            var PcntComplete = command.CreateParameter();
            PcntComplete.ParameterName = "$PcntComplete";
            command.Parameters.Add(PcntComplete);

            var UpdatedBy = command.CreateParameter();
            UpdatedBy.ParameterName = "$UpdatedBy";
            command.Parameters.Add(UpdatedBy);

            var UpdateDate = command.CreateParameter();
            UpdateDate.ParameterName = "$UpdateDate";
            command.Parameters.Add(UpdateDate);

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
                PermitNbr.Value = record.PermitNbr;
                PermitType.Value = record.PermitType;
                IssueDate.Value = record.IssueDate;
                PermitVal.Value = record.PermitVal;
                PermitStatus.Value = record.PermitStatus;
                PcntComplete.Value = record.PcntComplete;
                UpdatedBy.Value = record.UpdatedBy;
                UpdateDate.Value = record.UpdateDate;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
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

    public class PermitDetailHistory
    {
        [Ignore]
        public Guid Id { get; set; }
        [CsvHelper.Configuration.Attributes.Index(0)]
        public string PermitNbr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(1)]
        public string PermitItem { get; set; }
        [CsvHelper.Configuration.Attributes.Index(2)]
        public string ItemValue { get; set; }
        [Ignore]
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
                Delimiter = ",",
                MissingFieldFound = null,
                BadDataFound = null,
                CacheFields = true,
                Encoding = System.Text.Encoding.ASCII,
                TrimOptions = TrimOptions.InsideQuotes
            };

            using var transaction = await context.Database.BeginTransactionAsync();
            using var reader = new StreamReader(pathToCSV, System.Text.Encoding.ASCII);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecordsAsync<PermitDetailHistory>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into PermitDetailHistories (Id, PermitNbr, PermitItem, ItemValue, IngestedOn) " +
                $"values ($Id, $PermitNbr, $PermitItem, $ItemValue, $IngestedOn);";

            var Id = command.CreateParameter();
            Id.ParameterName = "$Id";
            command.Parameters.Add(Id);

            var PermitNbr = command.CreateParameter();
            PermitNbr.ParameterName = "$PermitNbr";
            command.Parameters.Add(PermitNbr);

            var PermitItem = command.CreateParameter();
            PermitItem.ParameterName = "$PermitItem";
            command.Parameters.Add(PermitItem);

            var ItemValue = command.CreateParameter();
            ItemValue.ParameterName = "$ItemValue";
            command.Parameters.Add(ItemValue);

            var IngestedOn = command.CreateParameter();
            IngestedOn.ParameterName = "$IngestedOn";
            command.Parameters.Add(IngestedOn);

            await foreach (var record in records)
            {
                record.Id = Guid.NewGuid();
                record.IngestedOn = DateTime.Now;
                record.TranslateFieldsUsingLookupsToText();

                Id.Value = record.Id;
                PermitNbr.Value = record.PermitNbr;
                PermitItem.Value = record.PermitItem;
                ItemValue.Value = record.ItemValue;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            PermitItem = GetPermitItem(PermitItem);

            return true;
        }

        public string GetPermitItem(string permitItem)
        {
            var permitItemNumber = int.Parse(permitItem.Trim());

            // Fancy switch statements too.
            return permitItemNumber switch
            {
                0 => null, // This indicates that there is no view. So we skip it.
                1 => "Applicant Name",
                10 => "Zoning",
                11 => "Other Description",
                12 => "Project Name",
                2 => "Applicant Address",
                21 => "Contractor Name",
                22 => "Contractor Address",
                23 => "Contractor Phone Nbr",
                24 => "Contractor License Nbr",
                3 => "Applicant Phone Nbr",
                31 => "Architect Name",
                32 => "Architect Address",
                33 => "Architect Phone Nbr",
                34 => "Architect License Nbr",
                40 => "Owner Name",
                41 => "Owner-Reported Value",
                42 => "Expiration Date",
                51 => "Square Feet",
                52 => "Nbr Stories",
                53 => "Nbr Units",
                54 => "Nbr Bedrooms",
                55 => "Nbr Buildings",
                56 => "Construction",
                57 => "Occupancy",
                58 => "Class",
                59 => "Water System",
                60 => "Sewer System",
                8 => "Property Address",
                9 => "Subdivision / Lot",
                _ => null,
            };
        }
    }
}