using CsvHelper;
using CsvHelper.Configuration;

using Flurl.Http;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace eRealProperty.Models
{
    public class ResidentialBuilding
    {
        public Guid Id { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string ParcelNumber { get; set; }
        public int BldgNbr { get; set; }
        public int NbrLivingUnits { get; set; }
        public string Address { get; set; }
        public string BuildingNumber { get; set; }
        public string Fraction { get; set; }
        public string DirectionPrefix { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string DirectionSuffix { get; set; }
        public string ZipCode { get; set; }
        public string Stories { get; set; }
        public string BldgGrade { get; set; }
        public int BldgGradeVar { get; set; }
        public int SqFt1stFloor { get; set; }
        public int SqFtHalfFloor { get; set; }
        public int SqFt2ndFloor { get; set; }
        public int SqFtUpperFloor { get; set; }
        public int SqFtUnfinFull { get; set; }
        public int SqFtUnfinHalf { get; set; }
        public int SqFtTotLiving { get; set; }
        public int SqFtTotBasement { get; set; }
        public int SqFtFinBasement { get; set; }
        public string FinBasementGrade { get; set; }
        public int SqFtGarageBasement { get; set; }
        public int SqFtGarageAttached { get; set; }
        public string DaylightBasement { get; set; }
        public int SqFtOpenPorch { get; set; }
        public int SqFtEnclosedPorch { get; set; }
        public int SqFtDeck { get; set; }
        public string HeatSystem { get; set; }
        public string HeatSource { get; set; }
        public int BrickStone { get; set; }
        public string ViewUtilization { get; set; }
        public int Bedrooms { get; set; }
        public int BathHalfCount { get; set; }
        public int Bath3qtrCount { get; set; }
        public int BathFullCount { get; set; }
        public int FpSingleStory { get; set; }
        public int FpMultiStory { get; set; }
        public int FpFreestanding { get; set; }
        public int FpAdditional { get; set; }
        public int YrBuilt { get; set; }
        public int YrRenovated { get; set; }
        public int PcntComplete { get; set; }
        public int Obsolescence { get; set; }
        public int PcntNetCondition { get; set; }
        public string Condition { get; set; }
        public int AddnlCost { get; set; }
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

            var transaction = context.Database.BeginTransaction();

            using (var reader = new StreamReader(pathToCSV))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var record = csv.GetRecord<ResidentialBuilding>();
                    record.Id = Guid.NewGuid();
                    record.IngestedOn = DateTime.Now;
                    record.TranslateFieldsUsingLookupsToText();

                    var command = context.Database.GetDbConnection().CreateCommand();
                    command.CommandText =
                        $"insert into ResidentialBuildings (Id, Major, Minor, ParcelNumber, BldgNbr, NbrLivingUnits, Address, BuildingNumber, Fraction, DirectionPrefix, StreetName, StreetType, DirectionSuffix, ZipCode, Stories, BldgGrade, BldgGradeVar, SqFt1stFloor, SqFtHalfFloor, SqFt2ndFloor, SqFtUpperFloor, SqFtUnfinFull, SqFtUnfinHalf, SqFtTotLiving, SqFtTotBasement, SqFtFinBasement, FinBasementGrade, SqFtGarageBasement, SqFtGarageAttached, DaylightBasement, SqFtOpenPorch, SqFtEnclosedPorch, SqFtDeck, HeatSystem, HeatSource, BrickStone, ViewUtilization, Bedrooms, BathHalfCount, Bath3qtrCount, BathFullCount, FpSingleStory, FpMultiStory, FpFreestanding, FpAdditional, YrBuilt, YrRenovated, PcntComplete, Obsolescence, PcntNetCondition, Condition, AddnlCost, IngestedOn) " +
                        $"values ($Id, $Major, $Minor, $ParcelNumber, $BldgNbr, $NbrLivingUnits, $Address, $BuildingNumber, $Fraction, $DirectionPrefix, $StreetName, $StreetType, $DirectionSuffix, $ZipCode, $Stories, $BldgGrade, $BldgGradeVar, $SqFt1stFloor, $SqFtHalfFloor, $SqFt2ndFloor, $SqFtUpperFloor, $SqFtUnfinFull, $SqFtUnfinHalf, $SqFtTotLiving, $SqFtTotBasement, $SqFtFinBasement, $FinBasementGrade, $SqFtGarageBasement, $SqFtGarageAttached, $DaylightBasement, $SqFtOpenPorch, $SqFtEnclosedPorch, $SqFtDeck, $HeatSystem, $HeatSource, $BrickStone, $ViewUtilization, $Bedrooms, $BathHalfCount, $Bath3qtrCount, $BathFullCount, $FpSingleStory, $FpMultiStory, $FpFreestanding, $FpAdditional, $YrBuilt, $YrRenovated, $PcntComplete, $Obsolescence, $PcntNetCondition, $Condition, $AddnlCost, $IngestedOn);";

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

                    var BldgNbr = command.CreateParameter();
                    BldgNbr.ParameterName = "$BldgNbr";
                    command.Parameters.Add(BldgNbr);
                    BldgNbr.Value = record.BldgNbr;

                    var NbrLivingUnits = command.CreateParameter();
                    NbrLivingUnits.ParameterName = "$NbrLivingUnits";
                    command.Parameters.Add(NbrLivingUnits);
                    NbrLivingUnits.Value = record.NbrLivingUnits;

                    var Address = command.CreateParameter();
                    Address.ParameterName = "$Address";
                    command.Parameters.Add(Address);
                    Address.Value = record.Address;

                    var BuildingNumber = command.CreateParameter();
                    BuildingNumber.ParameterName = "$BuildingNumber";
                    command.Parameters.Add(BuildingNumber);
                    BuildingNumber.Value = record.BuildingNumber;

                    var Fraction = command.CreateParameter();
                    Fraction.ParameterName = "$Fraction";
                    command.Parameters.Add(Fraction);
                    Fraction.Value = string.IsNullOrWhiteSpace(record?.Fraction) ? DBNull.Value : record.Fraction;

                    var DirectionPrefix = command.CreateParameter();
                    DirectionPrefix.ParameterName = "$DirectionPrefix";
                    command.Parameters.Add(DirectionPrefix);
                    DirectionPrefix.Value = record.DirectionPrefix;

                    var StreetName = command.CreateParameter();
                    StreetName.ParameterName = "$StreetName";
                    command.Parameters.Add(StreetName);
                    StreetName.Value = record.StreetName;

                    var StreetType = command.CreateParameter();
                    StreetType.ParameterName = "$StreetType";
                    command.Parameters.Add(StreetType);
                    StreetType.Value = record.StreetType;

                    var DirectionSuffix = command.CreateParameter();
                    DirectionSuffix.ParameterName = "$DirectionSuffix";
                    command.Parameters.Add(DirectionSuffix);
                    DirectionSuffix.Value = string.IsNullOrWhiteSpace(record?.DirectionSuffix) ? DBNull.Value : record.DirectionSuffix;

                    var ZipCode = command.CreateParameter();
                    ZipCode.ParameterName = "$ZipCode";
                    command.Parameters.Add(ZipCode);
                    ZipCode.Value = string.IsNullOrWhiteSpace(record?.ZipCode) ? DBNull.Value : record.ZipCode;

                    var Stories = command.CreateParameter();
                    Stories.ParameterName = "$Stories";
                    command.Parameters.Add(Stories);
                    Stories.Value = record.Stories;

                    var BldgGrade = command.CreateParameter();
                    BldgGrade.ParameterName = "$BldgGrade";
                    command.Parameters.Add(BldgGrade);
                    BldgGrade.Value = record.BldgGrade;

                    var BldgGradeVar = command.CreateParameter();
                    BldgGradeVar.ParameterName = "$BldgGradeVar";
                    command.Parameters.Add(BldgGradeVar);
                    BldgGradeVar.Value = record.BldgGradeVar;

                    var SqFt1stFloor = command.CreateParameter();
                    SqFt1stFloor.ParameterName = "$SqFt1stFloor";
                    command.Parameters.Add(SqFt1stFloor);
                    SqFt1stFloor.Value = record.SqFt1stFloor;

                    var SqFtHalfFloor = command.CreateParameter();
                    SqFtHalfFloor.ParameterName = "$SqFtHalfFloor";
                    command.Parameters.Add(SqFtHalfFloor);
                    SqFtHalfFloor.Value = record.SqFtHalfFloor;

                    var SqFt2ndFloor = command.CreateParameter();
                    SqFt2ndFloor.ParameterName = "$SqFt2ndFloor";
                    command.Parameters.Add(SqFt2ndFloor);
                    SqFt2ndFloor.Value = record.SqFt2ndFloor;

                    var SqFtUpperFloor = command.CreateParameter();
                    SqFtUpperFloor.ParameterName = "$SqFtUpperFloor";
                    command.Parameters.Add(SqFtUpperFloor);
                    SqFtUpperFloor.Value = record.SqFtUpperFloor;

                    var SqFtUnfinFull = command.CreateParameter();
                    SqFtUnfinFull.ParameterName = "$SqFtUnfinFull";
                    command.Parameters.Add(SqFtUnfinFull);
                    SqFtUnfinFull.Value = record.SqFtUnfinFull;

                    var SqFtUnfinHalf = command.CreateParameter();
                    SqFtUnfinHalf.ParameterName = "$SqFtUnfinHalf";
                    command.Parameters.Add(SqFtUnfinHalf);
                    SqFtUnfinHalf.Value = record.SqFtUnfinHalf;

                    var SqFtTotLiving = command.CreateParameter();
                    SqFtTotLiving.ParameterName = "$SqFtTotLiving";
                    command.Parameters.Add(SqFtTotLiving);
                    SqFtTotLiving.Value = record.SqFtTotLiving;

                    var SqFtTotBasement = command.CreateParameter();
                    SqFtTotBasement.ParameterName = "$SqFtTotBasement";
                    command.Parameters.Add(SqFtTotBasement);
                    SqFtTotBasement.Value = record.SqFtTotBasement;

                    var SqFtFinBasement = command.CreateParameter();
                    SqFtFinBasement.ParameterName = "$SqFtFinBasement";
                    command.Parameters.Add(SqFtFinBasement);
                    SqFtFinBasement.Value = record.SqFtFinBasement;

                    var FinBasementGrade = command.CreateParameter();
                    FinBasementGrade.ParameterName = "$FinBasementGrade";
                    command.Parameters.Add(FinBasementGrade);
                    FinBasementGrade.Value = string.IsNullOrWhiteSpace(record?.FinBasementGrade) ? DBNull.Value : record.FinBasementGrade;

                    var SqFtGarageBasement = command.CreateParameter();
                    SqFtGarageBasement.ParameterName = "$SqFtGarageBasement";
                    command.Parameters.Add(SqFtGarageBasement);
                    SqFtGarageBasement.Value = record.SqFtGarageBasement;

                    var SqFtGarageAttached = command.CreateParameter();
                    SqFtGarageAttached.ParameterName = "$SqFtGarageAttached";
                    command.Parameters.Add(SqFtGarageAttached);
                    SqFtGarageAttached.Value = record.SqFtGarageAttached;

                    var DaylightBasement = command.CreateParameter();
                    DaylightBasement.ParameterName = "$DaylightBasement";
                    command.Parameters.Add(DaylightBasement);
                    DaylightBasement.Value = string.IsNullOrWhiteSpace(record?.DaylightBasement) ? DBNull.Value : record.DaylightBasement;

                    var SqFtOpenPorch = command.CreateParameter();
                    SqFtOpenPorch.ParameterName = "$SqFtOpenPorch";
                    command.Parameters.Add(SqFtOpenPorch);
                    SqFtOpenPorch.Value = record.SqFtOpenPorch;

                    var SqFtEnclosedPorch = command.CreateParameter();
                    SqFtEnclosedPorch.ParameterName = "$SqFtEnclosedPorch";
                    command.Parameters.Add(SqFtEnclosedPorch);
                    SqFtEnclosedPorch.Value = record.SqFtEnclosedPorch;

                    var SqFtDeck = command.CreateParameter();
                    SqFtDeck.ParameterName = "$SqFtDeck";
                    command.Parameters.Add(SqFtDeck);
                    SqFtDeck.Value = record.SqFtDeck;

                    var HeatSystem = command.CreateParameter();
                    HeatSystem.ParameterName = "$HeatSystem";
                    command.Parameters.Add(HeatSystem);
                    HeatSystem.Value = string.IsNullOrWhiteSpace(record?.HeatSystem) ? DBNull.Value : record.HeatSystem;

                    var HeatSource = command.CreateParameter();
                    HeatSource.ParameterName = "$HeatSource";
                    command.Parameters.Add(HeatSource);
                    HeatSource.Value = string.IsNullOrWhiteSpace(record?.HeatSource) ? DBNull.Value : record.HeatSource;

                    var BrickStone = command.CreateParameter();
                    BrickStone.ParameterName = "$BrickStone";
                    command.Parameters.Add(BrickStone);
                    BrickStone.Value = record.BrickStone;

                    var ViewUtilization = command.CreateParameter();
                    ViewUtilization.ParameterName = "$ViewUtilization";
                    command.Parameters.Add(ViewUtilization);
                    ViewUtilization.Value = string.IsNullOrWhiteSpace(record?.ViewUtilization) ? DBNull.Value : record.ViewUtilization;

                    var Bedrooms = command.CreateParameter();
                    Bedrooms.ParameterName = "$Bedrooms";
                    command.Parameters.Add(Bedrooms);
                    Bedrooms.Value = record.Bedrooms;

                    var BathHalfCount = command.CreateParameter();
                    BathHalfCount.ParameterName = "$BathHalfCount";
                    command.Parameters.Add(BathHalfCount);
                    BathHalfCount.Value = record.BathHalfCount;

                    var Bath3qtrCount = command.CreateParameter();
                    Bath3qtrCount.ParameterName = "$Bath3qtrCount";
                    command.Parameters.Add(Bath3qtrCount);
                    Bath3qtrCount.Value = record.Bath3qtrCount;

                    var BathFullCount = command.CreateParameter();
                    BathFullCount.ParameterName = "$BathFullCount";
                    command.Parameters.Add(BathFullCount);
                    BathFullCount.Value = record.BathFullCount;

                    var FpSingleStory = command.CreateParameter();
                    FpSingleStory.ParameterName = "$FpSingleStory";
                    command.Parameters.Add(FpSingleStory);
                    FpSingleStory.Value = record.FpSingleStory;

                    var FpMultiStory = command.CreateParameter();
                    FpMultiStory.ParameterName = "$FpMultiStory";
                    command.Parameters.Add(FpMultiStory);
                    FpMultiStory.Value = record.FpMultiStory;

                    var FpFreestanding = command.CreateParameter();
                    FpFreestanding.ParameterName = "$FpFreestanding";
                    command.Parameters.Add(FpFreestanding);
                    FpFreestanding.Value = record.FpFreestanding;

                    var FpAdditional = command.CreateParameter();
                    FpAdditional.ParameterName = "$FpAdditional";
                    command.Parameters.Add(FpAdditional);
                    FpAdditional.Value = record.FpAdditional;

                    var YrBuilt = command.CreateParameter();
                    YrBuilt.ParameterName = "$YrBuilt";
                    command.Parameters.Add(YrBuilt);
                    YrBuilt.Value = record.YrBuilt;

                    var YrRenovated = command.CreateParameter();
                    YrRenovated.ParameterName = "$YrRenovated";
                    command.Parameters.Add(YrRenovated);
                    YrRenovated.Value = record.YrRenovated;

                    var PcntComplete = command.CreateParameter();
                    PcntComplete.ParameterName = "$PcntComplete";
                    command.Parameters.Add(PcntComplete);
                    PcntComplete.Value = record.PcntComplete;

                    var Obsolescence = command.CreateParameter();
                    Obsolescence.ParameterName = "$Obsolescence";
                    command.Parameters.Add(Obsolescence);
                    Obsolescence.Value = record.Obsolescence;

                    var PcntNetCondition = command.CreateParameter();
                    PcntNetCondition.ParameterName = "$PcntNetCondition";
                    command.Parameters.Add(PcntNetCondition);
                    PcntNetCondition.Value = record.PcntNetCondition;

                    var Condition = command.CreateParameter();
                    Condition.ParameterName = "$Condition";
                    command.Parameters.Add(Condition);
                    Condition.Value = record.Condition;

                    var AddnlCost = command.CreateParameter();
                    AddnlCost.ParameterName = "$AddnlCost";
                    command.Parameters.Add(AddnlCost);
                    AddnlCost.Value = record.AddnlCost;

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

        public static async Task<bool> IngestByParcelNumberAsync(string parcelNumber, eRealPropertyContext context)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_ResBldg.csv");

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
                var record = new ResidentialBuilding();

                while (csv.Read())
                {
                    record = csv.GetRecord<ResidentialBuilding>();
                    var major = parcelNumber.Substring(0, 6);
                    var minor = parcelNumber.Substring(6, 4);

                    if (record.Major == major && record.Minor == minor)
                    {
                        // Do something with the record.
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
