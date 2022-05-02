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
    public class RealPropertyAccountTaxYear
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        [Ignore]
        public string ParcelNumber { get; set; }
        public int TaxYr { get; set; }
        public int OmitYr { get; set; }
        public long ApprLandVal { get; set; }
        public long ApprImpsVal { get; set; }
        public long ApprImpIncr { get; set; }
        public long LandVal { get; set; }
        public long ImpsVal { get; set; }
        public string TaxValReason { get; set; }
        public string TaxStatus { get; set; }
        public int LevyCode { get; set; }
        public string ChangeDate { get; set; }
        public string ChangeDocId { get; set; }
        public string Reason { get; set; }
        public char SplitCode { get; set; }
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
                TrimOptions = TrimOptions.InsideQuotes
            };

            using var transaction = await context.Database.BeginTransactionAsync();
            using var reader = new StreamReader(pathToCSV);
            using var csv = new CsvReader(reader, config);
            {
                var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText =
                    $"insert into RealPropertyAccountTaxYears (Id, Major, Minor, ParcelNumber, TaxYr, OmitYr, ApprLandVal, ApprImpsVal, ApprImpIncr, LandVal, ImpsVal, TaxValReason, TaxStatus, LevyCode, ChangeDate, ChangeDocId, Reason, SplitCode, IngestedOn) " +
                    $"values ($Id, $Major, $Minor, $ParcelNumber, $TaxYr, $OmitYr, $ApprLandVal, $ApprImpsVal, $ApprImpIncr, $LandVal, $ImpsVal, $TaxValReason, $TaxStatus, $LevyCode, $ChangeDate, $ChangeDocId, $Reason, $SplitCode, $IngestedOn);";

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

                var TaxYr = command.CreateParameter();
                TaxYr.ParameterName = "$TaxYr";
                command.Parameters.Add(TaxYr);

                var OmitYr = command.CreateParameter();
                OmitYr.ParameterName = "$OmitYr";
                command.Parameters.Add(OmitYr);

                var ApprLandVal = command.CreateParameter();
                ApprLandVal.ParameterName = "$ApprLandVal";
                command.Parameters.Add(ApprLandVal);

                var ApprImpsVal = command.CreateParameter();
                ApprImpsVal.ParameterName = "$ApprImpsVal";
                command.Parameters.Add(ApprImpsVal);

                var ApprImpIncr = command.CreateParameter();
                ApprImpIncr.ParameterName = "$ApprImpIncr";
                command.Parameters.Add(ApprImpIncr);

                var LandVal = command.CreateParameter();
                LandVal.ParameterName = "$LandVal";
                command.Parameters.Add(LandVal);

                var ImpsVal = command.CreateParameter();
                ImpsVal.ParameterName = "$ImpsVal";
                command.Parameters.Add(ImpsVal);

                var TaxValReason = command.CreateParameter();
                TaxValReason.ParameterName = "$TaxValReason";
                command.Parameters.Add(TaxValReason);

                var TaxStatus = command.CreateParameter();
                TaxStatus.ParameterName = "$TaxStatus";
                command.Parameters.Add(TaxStatus);

                var LevyCode = command.CreateParameter();
                LevyCode.ParameterName = "$LevyCode";
                command.Parameters.Add(LevyCode);

                var ChangeDate = command.CreateParameter();
                ChangeDate.ParameterName = "$ChangeDate";
                command.Parameters.Add(ChangeDate);

                var ChangeDocId = command.CreateParameter();
                ChangeDocId.ParameterName = "$ChangeDocId";
                command.Parameters.Add(ChangeDocId);

                var Reason = command.CreateParameter();
                Reason.ParameterName = "$Reason";
                command.Parameters.Add(Reason);

                var SplitCode = command.CreateParameter();
                SplitCode.ParameterName = "$SplitCode";
                command.Parameters.Add(SplitCode);

                var IngestedOn = command.CreateParameter();
                IngestedOn.ParameterName = "$IngestedOn";
                command.Parameters.Add(IngestedOn);

                var records = csv.GetRecordsAsync<RealPropertyAccountTaxYear>();

                await foreach (var record in records)
                {
                    record.Id = Guid.NewGuid();
                    record.IngestedOn = DateTime.Now;
                    var checkTranslation = record.TranslateFieldsUsingLookupsToText();

                    Id.Value = record.Id;
                    Major.Value = record.Major;
                    Minor.Value = record.Minor;
                    ParcelNumber.Value = record.ParcelNumber;
                    TaxYr.Value = record.TaxYr;
                    OmitYr.Value = record.OmitYr;
                    ApprLandVal.Value = record.ApprLandVal;
                    ApprImpsVal.Value = record.ApprImpsVal;
                    ApprImpIncr.Value = record.ApprImpIncr;
                    LandVal.Value = record.LandVal;
                    ImpsVal.Value = record.ImpsVal;
                    TaxValReason.Value = string.IsNullOrWhiteSpace(record?.TaxValReason) ? DBNull.Value : record.TaxValReason;
                    TaxStatus.Value = string.IsNullOrWhiteSpace(record?.TaxStatus) ? DBNull.Value : record.TaxStatus;
                    LevyCode.Value = record.LevyCode;
                    ChangeDate.Value = record.ChangeDate;
                    ChangeDocId.Value = record.ChangeDocId;
                    Reason.Value = record.Reason;
                    SplitCode.Value = record.SplitCode;
                    IngestedOn.Value = record.IngestedOn;

                    await command.ExecuteNonQueryAsync();
                }
                await transaction.CommitAsync();
            }

            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            TaxStatus = GetTaxStatus();
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
            if (!string.IsNullOrWhiteSpace(TaxStatus))
            {
                return TaxStatus switch
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
