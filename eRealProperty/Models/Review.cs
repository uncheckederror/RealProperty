﻿using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

using Flurl.Http;

using Microsoft.EntityFrameworkCore;

using Serilog;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class Review
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        [CsvHelper.Configuration.Attributes.Index(0)]
        public string AppealNbr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(1)]
        public string Major { get; set; }
        [CsvHelper.Configuration.Attributes.Index(2)]
        public string Minor { get; set; }
        [Ignore]
        public string ParcelNumber { get; set; }
        [CsvHelper.Configuration.Attributes.Index(3)]
        public int BillYr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public string ReviewType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public string ReviewSource { get; set; }
        [CsvHelper.Configuration.Attributes.Index(6)]
        public string AssrRecommendation { get; set; }
        [CsvHelper.Configuration.Attributes.Index(7)]
        public string RespAppr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(8)]
        public string RelatedAppealNbr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(9)]
        public string Agent { get; set; }
        [CsvHelper.Configuration.Attributes.Index(10)]
        public string ValueType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(11)]
        public string AppellantReason { get; set; }
        [CsvHelper.Configuration.Attributes.Index(12)]
        public string StatusAssessor { get; set; }
        [CsvHelper.Configuration.Attributes.Index(13)]
        public string StatusStipulation { get; set; }
        [CsvHelper.Configuration.Attributes.Index(14)]
        public string StatusBoard { get; set; }
        [CsvHelper.Configuration.Attributes.Index(15)]
        public string StatusAssmtReview { get; set; }
        [CsvHelper.Configuration.Attributes.Index(16)]
        public DateTime HearingDate { get; set; }
        [CsvHelper.Configuration.Attributes.Index(17)]
        public string HearingType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(18)]
        public string HearingResult { get; set; }
        [CsvHelper.Configuration.Attributes.Index(19)]
        public string OrderTerm { get; set; }
        [CsvHelper.Configuration.Attributes.Index(20)]
        public string BoardReason { get; set; }
        [CsvHelper.Configuration.Attributes.Index(21)]
        public string AppealRecommended { get; set; }
        [CsvHelper.Configuration.Attributes.Index(22)]
        public string UpdatedBy { get; set; }
        [CsvHelper.Configuration.Attributes.Index(23)]
        public DateTime UpdateDate { get; set; }
        [CsvHelper.Configuration.Attributes.Index(24)]
        public string NoteId { get; set; }
        [Ignore]
        public DateTime IngestedOn { get; set; }
        [Ignore]
        [NotMapped]
        public ReviewDescription AppealedValue { get; set; }
        [Ignore]
        [NotMapped]
        public ReviewDescription FinalValue { get; set; }

        public static async Task<bool> IngestAsync(eRealPropertyContext context, string pathToCSV, CsvConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(pathToCSV) || context is null || config is null)
            {
                return false;
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            using var reader = new StreamReader(pathToCSV, config.Encoding);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecordsAsync<Review>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into Reviews (Id, AppealNbr, Major, Minor, BillYr, ReviewType, ReviewSource, AssrRecommendation, RespAppr, RelatedAppealNbr, Agent, ValueType, AppellantReason, StatusAssessor, StatusStipulation, StatusBoard, StatusAssmtReview, HearingDate, HearingType, HearingResult, OrderTerm, BoardReason, AppealRecommended, UpdatedBy, UpdateDate, NoteId, IngestedOn) " +
                $"values ($Id, $AppealNbr, $Major, $Minor, $BillYr, $ReviewType, $ReviewSource, $AssrRecommendation, $RespAppr, $RelatedAppealNbr, $Agent, $ValueType, $AppellantReason, $StatusAssessor, $StatusStipulation, $StatusBoard, $StatusAssmtReview, $HearingDate, $HearingType, $HearingResult, $OrderTerm, $BoardReason, $AppealRecommended, $UpdatedBy, $UpdateDate, $NoteId, $IngestedOn);";

            var Id = command.CreateParameter();
            Id.ParameterName = "$Id";
            command.Parameters.Add(Id);

            var AppealNbr = command.CreateParameter();
            AppealNbr.ParameterName = "AppealNbr";
            command.Parameters.Add(AppealNbr);

            var Major = command.CreateParameter();
            Major.ParameterName = "$Major";
            command.Parameters.Add(Major);

            var Minor = command.CreateParameter();
            Minor.ParameterName = "$Minor";
            command.Parameters.Add(Minor);

            var ParcelNumber = command.CreateParameter();
            ParcelNumber.ParameterName = "$ParcelNumber";
            command.Parameters.Add(ParcelNumber);

            var BillYr = command.CreateParameter();
            BillYr.ParameterName = "$BillYr";
            command.Parameters.Add(BillYr);

            var ReviewType = command.CreateParameter();
            ReviewType.ParameterName = "$ReviewType";
            command.Parameters.Add(ReviewType);

            var ReviewSource = command.CreateParameter();
            ReviewSource.ParameterName = "$ReviewSource";
            command.Parameters.Add(ReviewSource);

            var AssrRecommendation = command.CreateParameter();
            AssrRecommendation.ParameterName = "$AssrRecommendation";
            command.Parameters.Add(AssrRecommendation);

            var RespAppr = command.CreateParameter();
            RespAppr.ParameterName = "$RespAppr";
            command.Parameters.Add(RespAppr);

            var RelatedAppealNbr = command.CreateParameter();
            RelatedAppealNbr.ParameterName = "$RelatedAppealNbr";
            command.Parameters.Add(RelatedAppealNbr);

            var Agent = command.CreateParameter();
            Agent.ParameterName = "$Agent";
            command.Parameters.Add(Agent);

            var ValueType = command.CreateParameter();
            ValueType.ParameterName = "$ValueType";
            command.Parameters.Add(ValueType);

            var AppellantReason = command.CreateParameter();
            AppellantReason.ParameterName = "$AppellantReason";
            command.Parameters.Add(AppellantReason);

            var StatusAssessor = command.CreateParameter();
            StatusAssessor.ParameterName = "$StatusAssessor";
            command.Parameters.Add(StatusAssessor);

            var StatusStipulation = command.CreateParameter();
            StatusStipulation.ParameterName = "$StatusStipulation";
            command.Parameters.Add(StatusStipulation);

            var StatusBoard = command.CreateParameter();
            StatusBoard.ParameterName = "$StatusBoard";
            command.Parameters.Add(StatusBoard);

            var StatusAssmtReview = command.CreateParameter();
            StatusAssmtReview.ParameterName = "$StatusAssmtReview";
            command.Parameters.Add(StatusAssmtReview);

            var HearingDate = command.CreateParameter();
            HearingDate.ParameterName = "$HearingDate";
            command.Parameters.Add(HearingDate);

            var HearingType = command.CreateParameter();
            HearingType.ParameterName = "$HearingType";
            command.Parameters.Add(HearingType);

            var HearingResult = command.CreateParameter();
            HearingResult.ParameterName = "$HearingResult";
            command.Parameters.Add(HearingResult);

            var OrderTerm = command.CreateParameter();
            OrderTerm.ParameterName = "$OrderTerm";
            command.Parameters.Add(OrderTerm);

            var BoardReason = command.CreateParameter();
            BoardReason.ParameterName = "$BoardReason";
            command.Parameters.Add(BoardReason);

            var AppealRecommended = command.CreateParameter();
            AppealRecommended.ParameterName = "$AppealRecommended";
            command.Parameters.Add(AppealRecommended);

            var UpdatedBy = command.CreateParameter();
            UpdatedBy.ParameterName = "$UpdatedBy";
            command.Parameters.Add(UpdatedBy);

            var UpdateDate = command.CreateParameter();
            UpdateDate.ParameterName = "$UpdateDate";
            command.Parameters.Add(UpdateDate);

            var NoteId = command.CreateParameter();
            NoteId.ParameterName = "$NoteId";
            command.Parameters.Add(NoteId);

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
                AppealNbr.Value = string.IsNullOrWhiteSpace(record?.AppealNbr) ? DBNull.Value : record.AppealNbr;
                Major.Value = record.Major;
                Minor.Value = record.Minor;
                ParcelNumber.Value = record.ParcelNumber;
                BillYr.Value = record.BillYr;
                ReviewType.Value = string.IsNullOrWhiteSpace(record?.ReviewType) ? DBNull.Value : record.ReviewType;
                ReviewSource.Value = string.IsNullOrWhiteSpace(record?.ReviewSource) ? DBNull.Value : record.ReviewSource;
                AssrRecommendation.Value = string.IsNullOrWhiteSpace(record?.AssrRecommendation) ? DBNull.Value : record.AssrRecommendation;
                RespAppr.Value = string.IsNullOrWhiteSpace(record?.RespAppr) ? DBNull.Value : record.RespAppr;
                RelatedAppealNbr.Value = string.IsNullOrWhiteSpace(record?.RelatedAppealNbr) ? DBNull.Value : record.RelatedAppealNbr;
                Agent.Value = string.IsNullOrWhiteSpace(record?.Agent) ? DBNull.Value : record.Agent;
                ValueType.Value = string.IsNullOrWhiteSpace(record?.ValueType) ? DBNull.Value : record.ValueType;
                AppellantReason.Value = string.IsNullOrWhiteSpace(record?.AppellantReason) ? DBNull.Value : record.AppellantReason;
                StatusAssessor.Value = string.IsNullOrWhiteSpace(record?.StatusAssessor) ? DBNull.Value : record.StatusAssessor;
                StatusStipulation.Value = string.IsNullOrWhiteSpace(record?.StatusStipulation) ? DBNull.Value : record.StatusStipulation;
                StatusBoard.Value = string.IsNullOrWhiteSpace(record?.StatusBoard) ? DBNull.Value : record.StatusBoard;
                StatusAssmtReview.Value = string.IsNullOrWhiteSpace(record?.StatusAssmtReview) ? DBNull.Value : record.StatusAssmtReview;
                HearingType.Value = string.IsNullOrWhiteSpace(record?.HearingType) ? DBNull.Value : record.HearingType;
                HearingResult.Value = string.IsNullOrWhiteSpace(record?.HearingResult) ? DBNull.Value : record.HearingResult;
                HearingDate.Value = record.HearingDate;
                OrderTerm.Value = string.IsNullOrWhiteSpace(record?.OrderTerm) ? DBNull.Value : record.OrderTerm;
                BoardReason.Value = string.IsNullOrWhiteSpace(record?.BoardReason) ? DBNull.Value : record.BoardReason;
                AppealRecommended.Value = string.IsNullOrWhiteSpace(record?.AppealRecommended) ? DBNull.Value : record.AppealRecommended;
                UpdateDate.Value = record.UpdateDate;
                UpdatedBy.Value = string.IsNullOrWhiteSpace(record?.UpdatedBy) ? DBNull.Value : record.UpdatedBy;
                NoteId.Value = string.IsNullOrWhiteSpace(record?.NoteId) ? DBNull.Value : record.NoteId;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();

                count++;

                if (count % 10000 == 0)
                {
                    Log.Information($"Ingested {count} Reviews.");
                }
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            ReviewType = GetReviewType();
            ReviewSource = GetReviewSource();
            AssrRecommendation = GetAssrRecommendation();
            ValueType = GetValueType();
            AppellantReason = GetAppellantReason();
            StatusAssessor = GetStatus(StatusAssessor);
            StatusStipulation = GetStatus(StatusStipulation);
            StatusBoard = GetStatus(StatusBoard);
            StatusAssmtReview = GetStatus(StatusAssmtReview);
            HearingType = GetHearingType();
            HearingResult = GetHearingResult();
            OrderTerm = GetOrderTerm();
            BoardReason = GetBoardReason();

            return true;
        }

        public string GetBoardReason() =>
            BoardReason switch
            {
                "0" => null,
                "1" => "Comparable Property Sales",
                "10" => "Conflict of Interest: Uphold",
                "11" => "Improvement Obsolescence",
                "12" => "Value-Limiting Location",
                "13" => "Documented Non-Perc",
                "14" => "Neighborhood Nuisance",
                "15" => "Within Market: Uphold",
                "16" => "Total Value Supported: Uphold",
                "17" => "Value Allocated to Land, Imp Low",
                "18" => "Insufficient Evidence: Uphold",
                "19" => "Evidence Not Persuasive",
                "2" => "Purchase Price",
                "20" => "Other",
                "3" => "Land Features",
                "4" => "Deferred Maintenance",
                "5" => "Cost to Cure",
                "6" => "Expense to Develop",
                "7" => "Cost Approach to Value",
                "8" => "Income Analysis",
                "9" => "Incorrect Characteristic",
                _ => null,
            };

        public string GetOrderTerm() =>
            OrderTerm switch
            {
                "0" => "1 Year",
                "1" => "1 Year",
                "2" => "2 Years",
                _ => null,
            };

        public string GetHearingResult() =>
            HearingResult switch
            {
                "0" => null,
                "1" => "SUSTAIN",
                "2" => "REVISE",
                "20" => "INVALIDATED",
                "21" => "STIPULATED",
                "22" => "TRANSFERRED",
                "23" => "DISMISS",
                "24" => "WITHDRAWN",
                "3" => "REVISE, ASSESSOR RECOMMENDED",
                _ => null,
            };

        public string GetHearingType() =>
            HearingType switch
            {
                "0" => null,
                "1" => "FULL BOARD",
                "2" => "EXAMINER",
                "3" => "MINI BOARD A",
                "4" => "MINI BOARD B",
                "5" => "MINI BOARD C",
                "6" => "EQUALIZATION PANEL",
                "7" => "BOARD MEMBER",
                "8" => "REVIEW; NO HEARING",
                _ => null,
            };

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

        public string GetReviewType() =>
             ReviewType switch
             {
                 "0" => null, // This indicates that there is no view. So we skip it.
                 "1" => "Local Appeal",
                 "2" => "State Appeal",
                 "3" => "Court",
                 "4" => "Review - Assessment",
                 "5" => "Review - Characteristics",
                 "6" => "Review - Destruct",
                 _ => null,
             };


        public string GetReviewSource() =>
            ReviewSource switch
            {
                "0" => null,
                "1" => "Taxpayer",
                "2" => "Assessor",
                "3" => "Board",
                _ => null,
            };

        public string GetAssrRecommendation() =>
            AssrRecommendation switch
            {
                "0" => null,
                "1" => "Sustain",
                "2" => "Revise",
                "3" => "Stipulate",
                _ => null,
            };

        public string GetValueType() =>
            ValueType switch
            {
                "0" => null,
                "1" => "Assessed Value",
                "2" => "Appraised Value",
                "3" => "Other",
                _ => null,
            };

        public string GetAppellantReason() =>
            AppellantReason switch
            {
                "0" => null,
                "1" => "Appraised Value ",
                "2" => "Improvement Characteristics",
                "3" => "Site Characteristics",
                _ => null,
            };

        public string GetStatus(string status) =>
            status switch
            {
                "0" => null,
                "1" => "Active",
                "10 " => "Appeal Filed",
                "11 " => "Hearing Scheduled",
                "111" => "Stipulation Rejected",
                "115" => "Notice of Hearing",
                "116" => "Notice of Continuation of Hearing",
                "117" => "Late Evidence Objection",
                "118" => "Request for Continuance Received",
                "119" => "Continuance Granted",
                "12 " => "Hearing Completed",
                "120" => "Continuance Denied",
                "121" => "BTA Hearing Scheduled",
                "122" => "BTA Hearing Rescheduled",
                "123" => "BTA Hearing Cancelled",
                "13 " => "Board Order Issued",
                "14 " => "Ready to Mail",
                "15 " => "Received",
                "16" => "Assigned",
                "17" => "Case Prepared",
                "18" => "Case Reviewed",
                "19" => "Submitted to Board",
                "2" => "Withdrawn",
                "20" => "Stipulation Pending",
                "21" => "Stipulation Finalized",
                "22" => "Stipulation Voided",
                "23" => "Contact",
                "24" => "Other",
                "25" => "Decision Review Pending",
                "26" => "Stipulation Prepared",
                "27" => "Stipulation Reviewed",
                "28" => "Fast Track",
                "3" => "Void",
                "30" => "Hearing Cancelled",
                "31" => "Hearing Rescheduled",
                "32" => "Decision Cancelled Pending",
                "33" => "Decision Changed Pending",
                "34" => "AcctNbr Changed",
                "35" => "Amended",
                "36" => "Stipulation Reviewed Voided",
                "37" => "Stipulation Pending Voided",
                "38" => "Case Order Review Voided",
                "39" => "Case File Transferred Voided",
                "4" => "Denied",
                "40" => "Case Reviewed Voided",
                "41" => "Case Prepared Voided",
                "42" => "Stipulation Finalized Voided",
                "43" => "Stipulation Prepared Voided",
                "44" => "Set StatusAssmtReview to Active",
                "45" => "Petition Reactivated by the Board",
                "46" => "Appellant Info",
                "47" => "Appeal to State",
                "48" => "TRC",
                "49" => "Amended Appeal Record",
                "50" => "Change Assignment",
                "51" => "Threshold Reviewed",
                "52" => "Undeliverable email",
                "53" => "TP Letter of Exception",
                "54" => "TP Response to Exception",
                "55" => "BTA Scheduling",
                "56" => "Order Denying Petition for Review",
                "57" => "Exception Acknowledgement Letter",
                "58" => "Withdrawal Letter",
                "59" => "Direct Appeal Request",
                "60" => "Reconvene Request",
                "61" => "Response to Reconvene Request",
                "62" => "Taxpayer Signed Stipulation",
                "63" => "Order Denying Exceptions To Proposed Decision",
                "64" => "TP New Evidence",
                "65" => "TP Opening Brief/Statement",
                "66" => "TP Rebuttal Evidence",
                "67" => "TP Reply Brief/Statement",
                "68" => "TP Effort to Confer",
                "69" => "TP Evidence Previously Submitted to BOE",
                "7" => "Binder Prepared",
                "70" => "TP Motion",
                "71" => "TP Response to Motion",
                "8" => "Decision Reviewed",
                "80" => "Order Denying Exception Response",
                "85 " => "Order Granting Petition for Review",
                "86" => "Order Granting Exceptions to Proposed Decision",
                "87" => "Order Granting Exception Response",
                "88" => "Untimely Info",
                "9" => "Completed",
                "95" => "Order Denying Motion",
                "96" => "Order Granting Motion",
                _ => null,
            };
    }

    public class ReviewDescription
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        [CsvHelper.Configuration.Attributes.Index(0)]
        public string AppealNbr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(1)]
        public string EntryType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(2)]
        public DateTime EntryDate { get; set; }
        [CsvHelper.Configuration.Attributes.Index(3)]
        public string SourcePerson { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public string TargetPerson { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public string ValuationType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(6)]
        public int ApprLandVal { get; set; }
        [CsvHelper.Configuration.Attributes.Index(7)]
        public int ApprImpsVal { get; set; }
        [CsvHelper.Configuration.Attributes.Index(8)]
        public string UpdatedBy { get; set; }
        [CsvHelper.Configuration.Attributes.Index(9)]
        public DateTime UpdateDate { get; set; }
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
                $"insert into ReviewDescriptions (Id, AppealNbr, EntryType, EntryDate, SourcePerson, TargetPerson, ValuationType, ApprLandVal, ApprImpsVal, UpdatedBy, UpdateDate, IngestedOn) " +
                $"values ($Id, $AppealNbr, $EntryType, $EntryDate, $SourcePerson, $TargetPerson, $ValuationType, $ApprLandVal, $ApprImpsVal, $UpdatedBy, $UpdateDate, $IngestedOn);";

            var Id = command.CreateParameter();
            Id.ParameterName = "$Id";
            command.Parameters.Add(Id);

            var AppealNbr = command.CreateParameter();
            AppealNbr.ParameterName = "AppealNbr";
            command.Parameters.Add(AppealNbr);

            var EntryType = command.CreateParameter();
            EntryType.ParameterName = "$EntryType";
            command.Parameters.Add(EntryType);

            var EntryDate = command.CreateParameter();
            EntryDate.ParameterName = "$EntryDate";
            command.Parameters.Add(EntryDate);

            var SourcePerson = command.CreateParameter();
            SourcePerson.ParameterName = "$SourcePerson";
            command.Parameters.Add(SourcePerson);

            var TargetPerson = command.CreateParameter();
            TargetPerson.ParameterName = "$TargetPerson";
            command.Parameters.Add(TargetPerson);

            var ValuationType = command.CreateParameter();
            ValuationType.ParameterName = "$ValuationType";
            command.Parameters.Add(ValuationType);

            var ApprLandVal = command.CreateParameter();
            ApprLandVal.ParameterName = "$ApprLandVal";
            command.Parameters.Add(ApprLandVal);

            var ApprImpsVal = command.CreateParameter();
            ApprImpsVal.ParameterName = "$ApprImpsVal";
            command.Parameters.Add(ApprImpsVal);

            var UpdatedBy = command.CreateParameter();
            UpdatedBy.ParameterName = "$UpdatedBy";
            command.Parameters.Add(UpdatedBy);

            var UpdateDate = command.CreateParameter();
            UpdateDate.ParameterName = "$UpdateDate";
            command.Parameters.Add(UpdateDate);

            var IngestedOn = command.CreateParameter();
            IngestedOn.ParameterName = "$IngestedOn";
            command.Parameters.Add(IngestedOn);

            var records = csv.GetRecordsAsync<ReviewDescription>();

            var count = 0;

            await foreach (var record in records)
            {
                record.Id = Guid.NewGuid();
                record.IngestedOn = DateTime.Now;
                record.TranslateFieldsUsingLookupsToText();

                Id.Value = record.Id;
                AppealNbr.Value = string.IsNullOrWhiteSpace(record?.AppealNbr) ? DBNull.Value : record.AppealNbr; ;
                EntryType.Value = string.IsNullOrWhiteSpace(record?.EntryType) ? DBNull.Value : record.EntryType; ;
                EntryDate.Value = record.EntryDate;
                SourcePerson.Value = string.IsNullOrWhiteSpace(record?.SourcePerson) ? DBNull.Value : record.SourcePerson; ;
                TargetPerson.Value = string.IsNullOrWhiteSpace(record?.TargetPerson) ? DBNull.Value : record.TargetPerson; ;
                ValuationType.Value = string.IsNullOrWhiteSpace(record?.ValuationType) ? DBNull.Value : record.ValuationType;
                ApprLandVal.Value = record.ApprLandVal;
                ApprImpsVal.Value = record.ApprImpsVal;
                UpdatedBy.Value = string.IsNullOrWhiteSpace(record?.UpdatedBy) ? DBNull.Value : record.UpdatedBy; ;
                UpdateDate.Value = record.UpdateDate;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
                count++;

                if (count % 10000 == 0)
                {
                    Log.Information($"Ingested {count} Review Descriptions.");
                }
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            EntryType = GetStatus(EntryType);
            ValuationType = GetValuationType();

            return true;
        }

        public string GetValuationType() =>
            ValuationType switch
            {
                "0" => null,
                "1" => "Appealed Value",
                "2" => "Assessor Recommended Value",
                "3" => "Board Order Value",
                "4" => "Taxpayer Recommended",
                "5" => "Stipulated",
                _ => null,
            };

        public string GetStatus(string status) =>
            status switch
            {
                "0" => null,
                "1" => "Active",
                "10 " => "Appeal Filed",
                "11 " => "Hearing Scheduled",
                "111" => "Stipulation Rejected",
                "115" => "Notice of Hearing",
                "116" => "Notice of Continuation of Hearing",
                "117" => "Late Evidence Objection",
                "118" => "Request for Continuance Received",
                "119" => "Continuance Granted",
                "12 " => "Hearing Completed",
                "120" => "Continuance Denied",
                "121" => "BTA Hearing Scheduled",
                "122" => "BTA Hearing Rescheduled",
                "123" => "BTA Hearing Cancelled",
                "13 " => "Board Order Issued",
                "14 " => "Ready to Mail",
                "15 " => "Received",
                "16" => "Assigned",
                "17" => "Case Prepared",
                "18" => "Case Reviewed",
                "19" => "Submitted to Board",
                "2" => "Withdrawn",
                "20" => "Stipulation Pending",
                "21" => "Stipulation Finalized",
                "22" => "Stipulation Voided",
                "23" => "Contact",
                "24" => "Other",
                "25" => "Decision Review Pending",
                "26" => "Stipulation Prepared",
                "27" => "Stipulation Reviewed",
                "28" => "Fast Track",
                "3" => "Void",
                "30" => "Hearing Cancelled",
                "31" => "Hearing Rescheduled",
                "32" => "Decision Cancelled Pending",
                "33" => "Decision Changed Pending",
                "34" => "AcctNbr Changed",
                "35" => "Amended",
                "36" => "Stipulation Reviewed Voided",
                "37" => "Stipulation Pending Voided",
                "38" => "Case Order Review Voided",
                "39" => "Case File Transferred Voided",
                "4" => "Denied",
                "40" => "Case Reviewed Voided",
                "41" => "Case Prepared Voided",
                "42" => "Stipulation Finalized Voided",
                "43" => "Stipulation Prepared Voided",
                "44" => "Set StatusAssmtReview to Active",
                "45" => "Petition Reactivated by the Board",
                "46" => "Appellant Info",
                "47" => "Appeal to State",
                "48" => "TRC",
                "49" => "Amended Appeal Record",
                "50" => "Change Assignment",
                "51" => "Threshold Reviewed",
                "52" => "Undeliverable email",
                "53" => "TP Letter of Exception",
                "54" => "TP Response to Exception",
                "55" => "BTA Scheduling",
                "56" => "Order Denying Petition for Review",
                "57" => "Exception Acknowledgement Letter",
                "58" => "Withdrawal Letter",
                "59" => "Direct Appeal Request",
                "60" => "Reconvene Request",
                "61" => "Response to Reconvene Request",
                "62" => "Taxpayer Signed Stipulation",
                "63" => "Order Denying Exceptions To Proposed Decision",
                "64" => "TP New Evidence",
                "65" => "TP Opening Brief/Statement",
                "66" => "TP Rebuttal Evidence",
                "67" => "TP Reply Brief/Statement",
                "68" => "TP Effort to Confer",
                "69" => "TP Evidence Previously Submitted to BOE",
                "7" => "Binder Prepared",
                "70" => "TP Motion",
                "71" => "TP Response to Motion",
                "8" => "Decision Reviewed",
                "80" => "Order Denying Exception Response",
                "85 " => "Order Granting Petition for Review",
                "86" => "Order Granting Exceptions to Proposed Decision",
                "87" => "Order Granting Exception Response",
                "88" => "Untimely Info",
                "9" => "Completed",
                "95" => "Order Denying Motion",
                "96" => "Order Granting Motion",
                _ => null,
            };
    }
}
