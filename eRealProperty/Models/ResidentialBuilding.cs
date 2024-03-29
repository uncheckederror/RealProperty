﻿using CsvHelper;
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
using System.Linq;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class ResidentialBuilding
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
        public int BldgNbr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(3)]
        public int NbrLivingUnits { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public string Address { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public string BuildingNumber { get; set; }
        [CsvHelper.Configuration.Attributes.Index(6)]
        public string Fraction { get; set; }
        [CsvHelper.Configuration.Attributes.Index(7)]
        public string DirectionPrefix { get; set; }
        [CsvHelper.Configuration.Attributes.Index(8)]
        public string StreetName { get; set; }
        [CsvHelper.Configuration.Attributes.Index(9)]
        public string StreetType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(10)]
        public string DirectionSuffix { get; set; }
        [CsvHelper.Configuration.Attributes.Index(11)]
        public string ZipCode { get; set; }
        [CsvHelper.Configuration.Attributes.Index(12)]
        public string Stories { get; set; }
        [CsvHelper.Configuration.Attributes.Index(13)]
        public string BldgGrade { get; set; }
        [CsvHelper.Configuration.Attributes.Index(14)]
        public int BldgGradeVar { get; set; }
        [CsvHelper.Configuration.Attributes.Index(15)]
        public int SqFt1stFloor { get; set; }
        [CsvHelper.Configuration.Attributes.Index(16)]
        public int SqFtHalfFloor { get; set; }
        [CsvHelper.Configuration.Attributes.Index(17)]
        public int SqFt2ndFloor { get; set; }
        [CsvHelper.Configuration.Attributes.Index(18)]
        public int SqFtUpperFloor { get; set; }
        [CsvHelper.Configuration.Attributes.Index(19)]
        public int SqFtUnfinFull { get; set; }
        [CsvHelper.Configuration.Attributes.Index(20)]
        public int SqFtUnfinHalf { get; set; }
        [CsvHelper.Configuration.Attributes.Index(21)]
        public int SqFtTotLiving { get; set; }
        [CsvHelper.Configuration.Attributes.Index(22)]
        public int SqFtTotBasement { get; set; }
        [CsvHelper.Configuration.Attributes.Index(23)]
        public int SqFtFinBasement { get; set; }
        [CsvHelper.Configuration.Attributes.Index(24)]
        public string FinBasementGrade { get; set; }
        [CsvHelper.Configuration.Attributes.Index(25)]
        public int SqFtGarageBasement { get; set; }
        [CsvHelper.Configuration.Attributes.Index(26)]
        public int SqFtGarageAttached { get; set; }
        [CsvHelper.Configuration.Attributes.Index(27)]
        public string DaylightBasement { get; set; }
        [CsvHelper.Configuration.Attributes.Index(28)]
        public int SqFtOpenPorch { get; set; }
        [CsvHelper.Configuration.Attributes.Index(29)]
        public int SqFtEnclosedPorch { get; set; }
        [CsvHelper.Configuration.Attributes.Index(30)]
        public int SqFtDeck { get; set; }
        [CsvHelper.Configuration.Attributes.Index(31)]
        public string HeatSystem { get; set; }
        [CsvHelper.Configuration.Attributes.Index(32)]
        public string HeatSource { get; set; }
        [CsvHelper.Configuration.Attributes.Index(33)]
        public int BrickStone { get; set; }
        [CsvHelper.Configuration.Attributes.Index(34)]
        public string ViewUtilization { get; set; }
        [CsvHelper.Configuration.Attributes.Index(35)]
        public int Bedrooms { get; set; }
        [CsvHelper.Configuration.Attributes.Index(36)]
        public int BathHalfCount { get; set; }
        [CsvHelper.Configuration.Attributes.Index(37)]
        public int Bath3qtrCount { get; set; }
        [CsvHelper.Configuration.Attributes.Index(38)]
        public int BathFullCount { get; set; }
        [CsvHelper.Configuration.Attributes.Index(39)]
        public int FpSingleStory { get; set; }
        [CsvHelper.Configuration.Attributes.Index(40)]
        public int FpMultiStory { get; set; }
        [CsvHelper.Configuration.Attributes.Index(41)]
        public int FpFreestanding { get; set; }
        [CsvHelper.Configuration.Attributes.Index(42)]
        public int FpAdditional { get; set; }
        [CsvHelper.Configuration.Attributes.Index(43)]
        public int YrBuilt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(44)]
        public int YrRenovated { get; set; }
        [CsvHelper.Configuration.Attributes.Index(45)]
        public int PcntComplete { get; set; }
        [CsvHelper.Configuration.Attributes.Index(46)]
        public int Obsolescence { get; set; }
        [CsvHelper.Configuration.Attributes.Index(47)]
        public int PcntNetCondition { get; set; }
        [CsvHelper.Configuration.Attributes.Index(48)]
        public string Condition { get; set; }
        [CsvHelper.Configuration.Attributes.Index(49)]
        public int AddnlCost { get; set; }
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

            var records = csv.GetRecordsAsync<ResidentialBuilding>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into ResidentialBuildings (Id, Major, Minor, ParcelNumber, BldgNbr, NbrLivingUnits, Address, BuildingNumber, Fraction, DirectionPrefix, StreetName, StreetType, DirectionSuffix, ZipCode, Stories, BldgGrade, BldgGradeVar, SqFt1stFloor, SqFtHalfFloor, SqFt2ndFloor, SqFtUpperFloor, SqFtUnfinFull, SqFtUnfinHalf, SqFtTotLiving, SqFtTotBasement, SqFtFinBasement, FinBasementGrade, SqFtGarageBasement, SqFtGarageAttached, DaylightBasement, SqFtOpenPorch, SqFtEnclosedPorch, SqFtDeck, HeatSystem, HeatSource, BrickStone, ViewUtilization, Bedrooms, BathHalfCount, Bath3qtrCount, BathFullCount, FpSingleStory, FpMultiStory, FpFreestanding, FpAdditional, YrBuilt, YrRenovated, PcntComplete, Obsolescence, PcntNetCondition, Condition, AddnlCost, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $BldgNbr, $NbrLivingUnits, $Address, $BuildingNumber, $Fraction, $DirectionPrefix, $StreetName, $StreetType, $DirectionSuffix, $ZipCode, $Stories, $BldgGrade, $BldgGradeVar, $SqFt1stFloor, $SqFtHalfFloor, $SqFt2ndFloor, $SqFtUpperFloor, $SqFtUnfinFull, $SqFtUnfinHalf, $SqFtTotLiving, $SqFtTotBasement, $SqFtFinBasement, $FinBasementGrade, $SqFtGarageBasement, $SqFtGarageAttached, $DaylightBasement, $SqFtOpenPorch, $SqFtEnclosedPorch, $SqFtDeck, $HeatSystem, $HeatSource, $BrickStone, $ViewUtilization, $Bedrooms, $BathHalfCount, $Bath3qtrCount, $BathFullCount, $FpSingleStory, $FpMultiStory, $FpFreestanding, $FpAdditional, $YrBuilt, $YrRenovated, $PcntComplete, $Obsolescence, $PcntNetCondition, $Condition, $AddnlCost, $IngestedOn);";

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

            var BldgNbr = command.CreateParameter();
            BldgNbr.ParameterName = "$BldgNbr";
            command.Parameters.Add(BldgNbr);

            var NbrLivingUnits = command.CreateParameter();
            NbrLivingUnits.ParameterName = "$NbrLivingUnits";
            command.Parameters.Add(NbrLivingUnits);

            var Address = command.CreateParameter();
            Address.ParameterName = "$Address";
            command.Parameters.Add(Address);

            var BuildingNumber = command.CreateParameter();
            BuildingNumber.ParameterName = "$BuildingNumber";
            command.Parameters.Add(BuildingNumber);

            var Fraction = command.CreateParameter();
            Fraction.ParameterName = "$Fraction";
            command.Parameters.Add(Fraction);

            var DirectionPrefix = command.CreateParameter();
            DirectionPrefix.ParameterName = "$DirectionPrefix";
            command.Parameters.Add(DirectionPrefix);

            var StreetName = command.CreateParameter();
            StreetName.ParameterName = "$StreetName";
            command.Parameters.Add(StreetName);

            var StreetType = command.CreateParameter();
            StreetType.ParameterName = "$StreetType";
            command.Parameters.Add(StreetType);

            var DirectionSuffix = command.CreateParameter();
            DirectionSuffix.ParameterName = "$DirectionSuffix";
            command.Parameters.Add(DirectionSuffix);

            var ZipCode = command.CreateParameter();
            ZipCode.ParameterName = "$ZipCode";
            command.Parameters.Add(ZipCode);

            var Stories = command.CreateParameter();
            Stories.ParameterName = "$Stories";
            command.Parameters.Add(Stories);

            var BldgGrade = command.CreateParameter();
            BldgGrade.ParameterName = "$BldgGrade";
            command.Parameters.Add(BldgGrade);

            var BldgGradeVar = command.CreateParameter();
            BldgGradeVar.ParameterName = "$BldgGradeVar";
            command.Parameters.Add(BldgGradeVar);

            var SqFt1stFloor = command.CreateParameter();
            SqFt1stFloor.ParameterName = "$SqFt1stFloor";
            command.Parameters.Add(SqFt1stFloor);

            var SqFtHalfFloor = command.CreateParameter();
            SqFtHalfFloor.ParameterName = "$SqFtHalfFloor";
            command.Parameters.Add(SqFtHalfFloor);

            var SqFt2ndFloor = command.CreateParameter();
            SqFt2ndFloor.ParameterName = "$SqFt2ndFloor";
            command.Parameters.Add(SqFt2ndFloor);

            var SqFtUpperFloor = command.CreateParameter();
            SqFtUpperFloor.ParameterName = "$SqFtUpperFloor";
            command.Parameters.Add(SqFtUpperFloor);

            var SqFtUnfinFull = command.CreateParameter();
            SqFtUnfinFull.ParameterName = "$SqFtUnfinFull";
            command.Parameters.Add(SqFtUnfinFull);

            var SqFtUnfinHalf = command.CreateParameter();
            SqFtUnfinHalf.ParameterName = "$SqFtUnfinHalf";
            command.Parameters.Add(SqFtUnfinHalf);

            var SqFtTotLiving = command.CreateParameter();
            SqFtTotLiving.ParameterName = "$SqFtTotLiving";
            command.Parameters.Add(SqFtTotLiving);

            var SqFtTotBasement = command.CreateParameter();
            SqFtTotBasement.ParameterName = "$SqFtTotBasement";
            command.Parameters.Add(SqFtTotBasement);

            var SqFtFinBasement = command.CreateParameter();
            SqFtFinBasement.ParameterName = "$SqFtFinBasement";
            command.Parameters.Add(SqFtFinBasement);

            var FinBasementGrade = command.CreateParameter();
            FinBasementGrade.ParameterName = "$FinBasementGrade";
            command.Parameters.Add(FinBasementGrade);

            var SqFtGarageBasement = command.CreateParameter();
            SqFtGarageBasement.ParameterName = "$SqFtGarageBasement";
            command.Parameters.Add(SqFtGarageBasement);

            var SqFtGarageAttached = command.CreateParameter();
            SqFtGarageAttached.ParameterName = "$SqFtGarageAttached";
            command.Parameters.Add(SqFtGarageAttached);

            var DaylightBasement = command.CreateParameter();
            DaylightBasement.ParameterName = "$DaylightBasement";
            command.Parameters.Add(DaylightBasement);

            var SqFtOpenPorch = command.CreateParameter();
            SqFtOpenPorch.ParameterName = "$SqFtOpenPorch";
            command.Parameters.Add(SqFtOpenPorch);

            var SqFtEnclosedPorch = command.CreateParameter();
            SqFtEnclosedPorch.ParameterName = "$SqFtEnclosedPorch";
            command.Parameters.Add(SqFtEnclosedPorch);

            var SqFtDeck = command.CreateParameter();
            SqFtDeck.ParameterName = "$SqFtDeck";
            command.Parameters.Add(SqFtDeck);

            var HeatSystem = command.CreateParameter();
            HeatSystem.ParameterName = "$HeatSystem";
            command.Parameters.Add(HeatSystem);

            var HeatSource = command.CreateParameter();
            HeatSource.ParameterName = "$HeatSource";
            command.Parameters.Add(HeatSource);

            var BrickStone = command.CreateParameter();
            BrickStone.ParameterName = "$BrickStone";
            command.Parameters.Add(BrickStone);

            var ViewUtilization = command.CreateParameter();
            ViewUtilization.ParameterName = "$ViewUtilization";
            command.Parameters.Add(ViewUtilization);

            var Bedrooms = command.CreateParameter();
            Bedrooms.ParameterName = "$Bedrooms";
            command.Parameters.Add(Bedrooms);

            var BathHalfCount = command.CreateParameter();
            BathHalfCount.ParameterName = "$BathHalfCount";
            command.Parameters.Add(BathHalfCount);

            var Bath3qtrCount = command.CreateParameter();
            Bath3qtrCount.ParameterName = "$Bath3qtrCount";
            command.Parameters.Add(Bath3qtrCount);

            var BathFullCount = command.CreateParameter();
            BathFullCount.ParameterName = "$BathFullCount";
            command.Parameters.Add(BathFullCount);

            var FpSingleStory = command.CreateParameter();
            FpSingleStory.ParameterName = "$FpSingleStory";
            command.Parameters.Add(FpSingleStory);

            var FpMultiStory = command.CreateParameter();
            FpMultiStory.ParameterName = "$FpMultiStory";
            command.Parameters.Add(FpMultiStory);

            var FpFreestanding = command.CreateParameter();
            FpFreestanding.ParameterName = "$FpFreestanding";
            command.Parameters.Add(FpFreestanding);

            var FpAdditional = command.CreateParameter();
            FpAdditional.ParameterName = "$FpAdditional";
            command.Parameters.Add(FpAdditional);

            var YrBuilt = command.CreateParameter();
            YrBuilt.ParameterName = "$YrBuilt";
            command.Parameters.Add(YrBuilt);

            var YrRenovated = command.CreateParameter();
            YrRenovated.ParameterName = "$YrRenovated";
            command.Parameters.Add(YrRenovated);

            var PcntComplete = command.CreateParameter();
            PcntComplete.ParameterName = "$PcntComplete";
            command.Parameters.Add(PcntComplete);

            var Obsolescence = command.CreateParameter();
            Obsolescence.ParameterName = "$Obsolescence";
            command.Parameters.Add(Obsolescence);

            var PcntNetCondition = command.CreateParameter();
            PcntNetCondition.ParameterName = "$PcntNetCondition";
            command.Parameters.Add(PcntNetCondition);

            var Condition = command.CreateParameter();
            Condition.ParameterName = "$Condition";
            command.Parameters.Add(Condition);

            var AddnlCost = command.CreateParameter();
            AddnlCost.ParameterName = "$AddnlCost";
            command.Parameters.Add(AddnlCost);

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
                BldgNbr.Value = record.BldgNbr;
                NbrLivingUnits.Value = record.NbrLivingUnits;
                Address.Value = record.Address;
                BuildingNumber.Value = record.BuildingNumber;
                Fraction.Value = string.IsNullOrWhiteSpace(record?.Fraction) ? DBNull.Value : record.Fraction;
                DirectionPrefix.Value = record.DirectionPrefix;
                StreetName.Value = record.StreetName;
                StreetType.Value = record.StreetType;
                DirectionSuffix.Value = string.IsNullOrWhiteSpace(record?.DirectionSuffix) ? DBNull.Value : record.DirectionSuffix;
                ZipCode.Value = string.IsNullOrWhiteSpace(record?.ZipCode) ? DBNull.Value : record.ZipCode;
                Stories.Value = record.Stories;
                BldgGrade.Value = record.BldgGrade;
                BldgGradeVar.Value = record.BldgGradeVar;
                SqFt1stFloor.Value = record.SqFt1stFloor;
                SqFtHalfFloor.Value = record.SqFtHalfFloor;
                SqFt2ndFloor.Value = record.SqFt2ndFloor;
                SqFtUpperFloor.Value = record.SqFtUpperFloor;
                SqFtUnfinFull.Value = record.SqFtUnfinFull;
                SqFtUnfinHalf.Value = record.SqFtUnfinHalf;
                SqFtTotLiving.Value = record.SqFtTotLiving;
                SqFtTotBasement.Value = record.SqFtTotBasement;
                SqFtFinBasement.Value = record.SqFtFinBasement;
                FinBasementGrade.Value = string.IsNullOrWhiteSpace(record?.FinBasementGrade) ? DBNull.Value : record.FinBasementGrade;
                SqFtGarageBasement.Value = record.SqFtGarageBasement;
                SqFtGarageAttached.Value = record.SqFtGarageAttached;
                DaylightBasement.Value = string.IsNullOrWhiteSpace(record?.DaylightBasement) ? DBNull.Value : record.DaylightBasement;
                SqFtOpenPorch.Value = record.SqFtOpenPorch;
                SqFtEnclosedPorch.Value = record.SqFtEnclosedPorch;
                SqFtDeck.Value = record.SqFtDeck;
                HeatSystem.Value = string.IsNullOrWhiteSpace(record?.HeatSystem) ? DBNull.Value : record.HeatSystem;
                HeatSource.Value = string.IsNullOrWhiteSpace(record?.HeatSource) ? DBNull.Value : record.HeatSource;
                BrickStone.Value = record.BrickStone;
                ViewUtilization.Value = string.IsNullOrWhiteSpace(record?.ViewUtilization) ? DBNull.Value : record.ViewUtilization;
                Bedrooms.Value = record.Bedrooms;
                BathHalfCount.Value = record.BathHalfCount;
                Bath3qtrCount.Value = record.Bath3qtrCount;
                BathFullCount.Value = record.BathFullCount;
                FpSingleStory.Value = record.FpSingleStory;
                FpMultiStory.Value = record.FpMultiStory;
                FpFreestanding.Value = record.FpFreestanding;
                FpAdditional.Value = record.FpAdditional;
                YrBuilt.Value = record.YrBuilt;
                YrRenovated.Value = record.YrRenovated;
                PcntComplete.Value = record.PcntComplete;
                Obsolescence.Value = record.Obsolescence;
                PcntNetCondition.Value = record.PcntNetCondition;
                Condition.Value = record.Condition;
                AddnlCost.Value = record.AddnlCost;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            BldgGrade = GetBuildingGrade();
            FinBasementGrade = GetFinishedBasementGrade();
            HeatSystem = GetHeatSystem();
            HeatSource = GetHeatSource();
            Condition = GetCondition();
            return true;
        }

        public string GetBuildingGrade()
        {
            return BldgGrade switch
            {
                "0" => null,
                "1" => "1 Cabin",
                "2" => "2 Substandard",
                "3" => "3 Poor",
                "4" => "4 Low",
                "5" => "5 Fair",
                "6" => "6 Low Average",
                "7" => "7 Average",
                "8" => "8 Good",
                "9" => "9 Better",
                "10" => "10 Very Good",
                "11" => "11 Excellent",
                "12" => "12 Luxury",
                "13" => "13 Mansion",
                "20" => "Exceptional Properties",
                _ => string.Empty,
            };
        }

        public string GetFinishedBasementGrade()
        {
            return FinBasementGrade switch
            {
                "0" => null,
                "1" => "1 Cabin",
                "2" => "2 Substandard",
                "3" => "3 Poor",
                "4" => "4 Low",
                "5" => "5 Fair",
                "6" => "6 Low Average",
                "7" => "7 Average",
                "8" => "8 Good",
                "9" => "9 Better",
                "10" => "10 Very Good",
                "11" => "11 Excellent",
                "12" => "12 Luxury",
                "13" => "13 Mansion",
                "20" => "Exceptional Properties",
                _ => string.Empty,
            };
        }

        public string GetHeatSource()
        {
            return HeatSource switch
            {
                "0" => null,
                "1" => "Oil",
                "2" => "Gas",
                "3" => "Electricity",
                "4" => "Oil or Solar",
                "5" => "Gas or Solar",
                "6" => "Electricity or Solar",
                "7" => "Other",
                _ => string.Empty,
            };
        }

        public string GetCondition()
        {
            return Condition switch
            {
                "0" => null,
                "1" => "Poor",
                "2" => "Fair",
                "3" => "Average",
                "4" => "Good",
                "5" => "Very Good",
                _ => string.Empty,
            };
        }

        public string GetHeatSystem()
        {
            return HeatSystem switch
            {
                "0" => null,
                "1" => "Floor or Wall",
                "2" => "Gravity",
                "3" => "Radiant",
                "4" => "Electric Baseboard",
                "5" => "Forced Air",
                "6" => "Hot Water",
                "7" => "Heat Pump",
                "8" => "Other",
                _ => string.Empty,
            };
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