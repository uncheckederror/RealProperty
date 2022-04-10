using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Flurl.Http;

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

                var transaction = context.Database.BeginTransaction();
                var record = new RealPropertyAccount();

                while (csv.Read())
                {
                    record.Id = Guid.NewGuid();
                    record.AcctNbr = csv.GetField<string>("AcctNbr");
                    record.AddrLine = csv.GetField<string>("AddrLine");
                    record.ApprImpsVal = csv.GetField<string>("ApprImpsVal");
                    record.ApprLandVal = csv.GetField<string>("ApprLandVal");
                    record.AttnLine = csv.GetField<string>("AttnLine");
                    record.BillYr = csv.GetField<int>("BillYr");
                    record.CityState = csv.GetField<string>("CityState");
                    record.TaxStat = csv.GetField<string>("TaxStat");
                    record.LevyCode = csv.GetField<string>("LevyCode");
                    record.Major = csv.GetField<string>("Major");
                    record.Minor = csv.GetField<string>("Minor");
                    record.NewConstructionFlag = csv.GetField<string>("NewConstructionFlag");
                    record.TaxableImpsVal = csv.GetField<string>("TaxableImpsVal");
                    record.TaxableLandVal = csv.GetField<string>("TaxableLandVal");
                    record.TaxValReason = csv.GetField<string>("TaxValReason");
                    record.ZipCode = csv.GetField<string>("ZipCode");
                    record.IngestedOn = DateTime.Now;

                    record.TranslateFieldsUsingLookupsToText();

                    var command = context.Database.GetDbConnection().CreateCommand();
                    command.CommandText =
                        $"insert into RealPropertyAccounts (Id, AcctNbr, Major, Minor, ParcelNumber, AttnLine, AddrLine, CityState, ZipCode, LevyCode, TaxStat, BillYr, NewConstructionFlag, TaxValReason, ApprLandVal, ApprImpsVal, TaxableLandVal, TaxableImpsVal, IngestedOn) " +
                        $"values ($Id, $AcctNbr, $Major, $Minor, $ParcelNumber, $AttnLine, $AddrLine, $CityState, $ZipCode, $LevyCode, $TaxStat, $BillYr, $NewConstructionFlag, $TaxValReason, $ApprImpsVal, $ApprImpsVal, $TaxableLandVal, $TaxableImpsVal, $IngestedOn);";

                    var Id = command.CreateParameter();
                    Id.ParameterName = "$Id";
                    command.Parameters.Add(Id);
                    Id.Value = record.Id;

                    var AcctNbr = command.CreateParameter();
                    AcctNbr.ParameterName = "$AcctNbr";
                    command.Parameters.Add(AcctNbr);
                    AcctNbr.Value = record.AcctNbr;

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

                    var AttnLine = command.CreateParameter();
                    AttnLine.ParameterName = "$AttnLine";
                    command.Parameters.Add(AttnLine);
                    AttnLine.Value = string.IsNullOrWhiteSpace(record?.AttnLine) ? DBNull.Value : record.AttnLine;

                    var AddrLine = command.CreateParameter();
                    AddrLine.ParameterName = "$AddrLine";
                    command.Parameters.Add(AddrLine);
                    AddrLine.Value = record.AddrLine;

                    var CityState = command.CreateParameter();
                    CityState.ParameterName = "$CityState";
                    command.Parameters.Add(CityState);
                    CityState.Value = record.CityState;

                    var ZipCode = command.CreateParameter();
                    ZipCode.ParameterName = "$ZipCode";
                    command.Parameters.Add(ZipCode);
                    ZipCode.Value = record.ZipCode;

                    var LevyCode = command.CreateParameter();
                    LevyCode.ParameterName = "$LevyCode";
                    command.Parameters.Add(LevyCode);
                    LevyCode.Value = record.LevyCode;

                    var TaxStat = command.CreateParameter();
                    TaxStat.ParameterName = "$TaxStat";
                    command.Parameters.Add(TaxStat);
                    TaxStat.Value = record.TaxStat;

                    var BillYr = command.CreateParameter();
                    BillYr.ParameterName = "$BillYr";
                    command.Parameters.Add(BillYr);
                    BillYr.Value = record.BillYr;

                    var NewConstructionFlag = command.CreateParameter();
                    NewConstructionFlag.ParameterName = "$NewConstructionFlag";
                    command.Parameters.Add(NewConstructionFlag);
                    NewConstructionFlag.Value = record.NewConstructionFlag;

                    var TaxValReason = command.CreateParameter();
                    TaxValReason.ParameterName = "$TaxValReason";
                    command.Parameters.Add(TaxValReason);
                    TaxValReason.Value = string.IsNullOrWhiteSpace(record?.TaxValReason) ? DBNull.Value : record.TaxValReason;

                    var ApprLandVal = command.CreateParameter();
                    ApprLandVal.ParameterName = "$ApprLandVal";
                    command.Parameters.Add(ApprLandVal);
                    ApprLandVal.Value = record.ApprLandVal;

                    var ApprImpsVal = command.CreateParameter();
                    ApprImpsVal.ParameterName = "$ApprImpsVal";
                    command.Parameters.Add(ApprImpsVal);
                    ApprImpsVal.Value = record.ApprImpsVal;

                    var TaxableLandVal = command.CreateParameter();
                    TaxableLandVal.ParameterName = "$TaxableLandVal";
                    command.Parameters.Add(TaxableLandVal);
                    TaxableLandVal.Value = record.TaxableLandVal;

                    var TaxableImpsVal = command.CreateParameter();
                    TaxableImpsVal.ParameterName = "$TaxableImpsVal";
                    command.Parameters.Add(TaxableImpsVal);
                    TaxableImpsVal.Value = record.TaxableImpsVal;

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
