using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

using IndexAttribute = CsvHelper.Configuration.Attributes.IndexAttribute;

namespace eRealProperty.Models
{
    public class RealPropertyAccount
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        [Index(0)]
        public string AcctNbr { get; set; }
        [Index(1)]
        public string Major { get; set; }
        [Index(2)]
        public string Minor { get; set; }
        [Ignore]
        public string ParcelNumber { get; set; }
        [Index(3)]
        public string AttnLine { get; set; }
        [Index(4)]
        public string AddrLine { get; set; }
        [Index(5)]
        public string CityState { get; set; }
        [Index(6)]
        public string ZipCode { get; set; }
        [Index(7)]
        public string LevyCode { get; set; }
        [Index(8)]
        public string TaxStat { get; set; }
        [Index(9)]
        public int BillYr { get; set; }
        [Index(10)]
        public string NewConstructionFlag { get; set; }
        [Index(11)]
        public string TaxValReason { get; set; }
        [Index(12)]
        public string ApprLandVal { get; set; }
        [Index(13)]
        public string ApprImpsVal { get; set; }
        [Index(14)]
        public string TaxableLandVal { get; set; }
        [Index(15)]
        public string TaxableImpsVal { get; set; }
        [Ignore]
        public DateTime IngestedOn { get; set; }
        // From the legal description file.
        [Ignore]
        public string LegalDescription { get; set; }

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
                $"insert into RealPropertyAccounts (Id, AcctNbr, Major, Minor, ParcelNumber, AttnLine, AddrLine, CityState, ZipCode, LevyCode, TaxStat, BillYr, NewConstructionFlag, TaxValReason, ApprLandVal, ApprImpsVal, TaxableLandVal, TaxableImpsVal, IngestedOn) " +
                $"values ($Id, $AcctNbr, $Major, $Minor, $ParcelNumber, $AttnLine, $AddrLine, $CityState, $ZipCode, $LevyCode, $TaxStat, $BillYr, $NewConstructionFlag, $TaxValReason, $ApprImpsVal, $ApprImpsVal, $TaxableLandVal, $TaxableImpsVal, $IngestedOn);";

            var Id = command.CreateParameter();
            Id.ParameterName = "$Id";
            command.Parameters.Add(Id);

            var AcctNbr = command.CreateParameter();
            AcctNbr.ParameterName = "$AcctNbr";
            command.Parameters.Add(AcctNbr);

            var Major = command.CreateParameter();
            Major.ParameterName = "$Major";
            command.Parameters.Add(Major);

            var Minor = command.CreateParameter();
            Minor.ParameterName = "$Minor";
            command.Parameters.Add(Minor);

            var ParcelNumber = command.CreateParameter();
            ParcelNumber.ParameterName = "$ParcelNumber";
            command.Parameters.Add(ParcelNumber);

            var AttnLine = command.CreateParameter();
            AttnLine.ParameterName = "$AttnLine";
            command.Parameters.Add(AttnLine);

            var AddrLine = command.CreateParameter();
            AddrLine.ParameterName = "$AddrLine";
            command.Parameters.Add(AddrLine);

            var CityState = command.CreateParameter();
            CityState.ParameterName = "$CityState";
            command.Parameters.Add(CityState);

            var ZipCode = command.CreateParameter();
            ZipCode.ParameterName = "$ZipCode";
            command.Parameters.Add(ZipCode);

            var LevyCode = command.CreateParameter();
            LevyCode.ParameterName = "$LevyCode";
            command.Parameters.Add(LevyCode);

            var TaxStat = command.CreateParameter();
            TaxStat.ParameterName = "$TaxStat";
            command.Parameters.Add(TaxStat);

            var BillYr = command.CreateParameter();
            BillYr.ParameterName = "$BillYr";
            command.Parameters.Add(BillYr);

            var NewConstructionFlag = command.CreateParameter();
            NewConstructionFlag.ParameterName = "$NewConstructionFlag";
            command.Parameters.Add(NewConstructionFlag);

            var TaxValReason = command.CreateParameter();
            TaxValReason.ParameterName = "$TaxValReason";
            command.Parameters.Add(TaxValReason);

            var ApprLandVal = command.CreateParameter();
            ApprLandVal.ParameterName = "$ApprLandVal";
            command.Parameters.Add(ApprLandVal);

            var ApprImpsVal = command.CreateParameter();
            ApprImpsVal.ParameterName = "$ApprImpsVal";
            command.Parameters.Add(ApprImpsVal);

            var TaxableLandVal = command.CreateParameter();
            TaxableLandVal.ParameterName = "$TaxableLandVal";
            command.Parameters.Add(TaxableLandVal);

            var TaxableImpsVal = command.CreateParameter();
            TaxableImpsVal.ParameterName = "$TaxableImpsVal";
            command.Parameters.Add(TaxableImpsVal);

            var IngestedOn = command.CreateParameter();
            IngestedOn.ParameterName = "$IngestedOn";
            command.Parameters.Add(IngestedOn);

            var records = csv.GetRecordsAsync<RealPropertyAccount>();

            await foreach (var record in records)
            {
                record.Id = Guid.NewGuid();
                record.IngestedOn = DateTime.Now;
                record.TranslateFieldsUsingLookupsToText();

                Id.Value = record.Id;
                AcctNbr.Value = record.AcctNbr;
                Major.Value = record.Major;
                Minor.Value = record.Minor;
                ParcelNumber.Value = record.ParcelNumber;
                AttnLine.Value = string.IsNullOrWhiteSpace(record?.AttnLine) ? DBNull.Value : record.AttnLine;
                AddrLine.Value = record.AddrLine;
                CityState.Value = record.CityState;
                ZipCode.Value = record.ZipCode;
                LevyCode.Value = record.LevyCode;
                TaxStat.Value = record.TaxStat;
                BillYr.Value = record.BillYr;
                NewConstructionFlag.Value = record.NewConstructionFlag;
                TaxValReason.Value = string.IsNullOrWhiteSpace(record?.TaxValReason) ? DBNull.Value : record.TaxValReason;
                ApprLandVal.Value = record.ApprLandVal;
                ApprImpsVal.Value = record.ApprImpsVal;
                TaxableLandVal.Value = record.TaxableLandVal;
                TaxableImpsVal.Value = record.TaxableImpsVal;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
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
