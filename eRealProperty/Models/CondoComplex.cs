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
    public class CondoComplex
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        [CsvHelper.Configuration.Attributes.Index(0)]
        public string Major { get; set; }
        [CsvHelper.Configuration.Attributes.Index(1)]
        public string ComplexType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(2)]
        public string ComplexDescr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(3)]
        public int NbrBldgs { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public int NbrStories { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public int NbrUnits { get; set; }
        [CsvHelper.Configuration.Attributes.Index(6)]
        public int AvgUnitSize { get; set; }
        [CsvHelper.Configuration.Attributes.Index(7)]
        public int LandPerUnit { get; set; }
        [CsvHelper.Configuration.Attributes.Index(8)]
        public string ProjectLocation { get; set; }
        [CsvHelper.Configuration.Attributes.Index(9)]
        public string ProjectAppeal { get; set; }
        [CsvHelper.Configuration.Attributes.Index(10)]
        public int PcntWithView { get; set; }
        [CsvHelper.Configuration.Attributes.Index(11)]
        public string ConstrClass { get; set; }
        [CsvHelper.Configuration.Attributes.Index(12)]
        public string BldgQuality { get; set; }
        [CsvHelper.Configuration.Attributes.Index(13)]
        public string Condition { get; set; }
        [CsvHelper.Configuration.Attributes.Index(14)]
        public int YrBuilt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(15)]
        public int EffYr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(16)]
        public int PcntComplete { get; set; }
        [CsvHelper.Configuration.Attributes.Index(17)]
        public string Elevators { get; set; }
        [CsvHelper.Configuration.Attributes.Index(18)]
        public string SectySystem { get; set; }
        [CsvHelper.Configuration.Attributes.Index(19)]
        public string Fireplace { get; set; }
        [CsvHelper.Configuration.Attributes.Index(20)]
        public string Laundry { get; set; }
        [CsvHelper.Configuration.Attributes.Index(21)]
        public string AptConversion { get; set; }
        [CsvHelper.Configuration.Attributes.Index(22)]
        public string CondoLandType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(23)]
        public string Address { get; set; }
        [CsvHelper.Configuration.Attributes.Index(24)]
        public string BuildingNumber { get; set; }
        [CsvHelper.Configuration.Attributes.Index(25)]
        public string Fraction { get; set; }
        [CsvHelper.Configuration.Attributes.Index(26)]
        public string DirectionPrefix { get; set; }
        [CsvHelper.Configuration.Attributes.Index(27)]
        public string StreetName { get; set; }
        [CsvHelper.Configuration.Attributes.Index(28)]
        public string StreetType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(29)]
        public string DirectionSuffix { get; set; }
        [CsvHelper.Configuration.Attributes.Index(30)]
        public string ZipCode { get; set; }
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

            var records = csv.GetRecordsAsync<CondoComplex>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into CondoComplexes (Id, Major, ComplexType, ComplexDescr, NbrBldgs, NbrStories, NbrUnits, AvgUnitSize, LandPerUnit, ProjectLocation, ProjectAppeal, PcntWithView, ConstrClass, BldgQuality, Condition, YrBuilt, EffYr, PcntComplete, Elevators, SectySystem, Fireplace, Laundry, AptConversion, CondoLandType, Address, BuildingNumber, Fraction, DirectionPrefix, StreetName, StreetType, DirectionSuffix, ZipCode, IngestedOn) " +
                $"values ($Id, $Major, $ComplexType, $ComplexDescr, $NbrBldgs, $NbrStories, $NbrUnits, $AvgUnitSize, $LandPerUnit, $ProjectLocation, $ProjectAppeal, $PcntWithView, $ConstrClass, $BldgQuality, $Condition, $YrBuilt, $EffYr, $PcntComplete, $Elevators, $SectySystem, $Fireplace, $Laundry, $AptConversion, $CondoLandType, $Address, $BuildingNumber, $Fraction, $DirectionPrefix, $StreetName, $StreetType, $DirectionSuffix, $ZipCode, $IngestedOn);";

            var Id = command.CreateParameter();
            Id.ParameterName = "$Id";
            command.Parameters.Add(Id);

            var Major = command.CreateParameter();
            Major.ParameterName = "$Major";
            command.Parameters.Add(Major);

            var ComplexType = command.CreateParameter();
            ComplexType.ParameterName = "$ComplexType";
            command.Parameters.Add(ComplexType);

            var ComplexDescr = command.CreateParameter();
            ComplexDescr.ParameterName = "$ComplexDescr";
            command.Parameters.Add(ComplexDescr);

            var NbrBldgs = command.CreateParameter();
            NbrBldgs.ParameterName = "$NbrBldgs";
            command.Parameters.Add(NbrBldgs);

            var NbrStories = command.CreateParameter();
            NbrStories.ParameterName = "$NbrStories";
            command.Parameters.Add(NbrStories);

            var NbrUnits = command.CreateParameter();
            NbrUnits.ParameterName = "$NbrUnits";
            command.Parameters.Add(NbrUnits);

            var AvgUnitSize = command.CreateParameter();
            AvgUnitSize.ParameterName = "$AvgUnitSize";
            command.Parameters.Add(AvgUnitSize);

            var LandPerUnit = command.CreateParameter();
            LandPerUnit.ParameterName = "$LandPerUnit";
            command.Parameters.Add(LandPerUnit);

            var ProjectLocation = command.CreateParameter();
            ProjectLocation.ParameterName = "$ProjectLocation";
            command.Parameters.Add(ProjectLocation);

            var ProjectAppeal = command.CreateParameter();
            ProjectAppeal.ParameterName = "$ProjectAppeal";
            command.Parameters.Add(ProjectAppeal);

            var PcntWithView = command.CreateParameter();
            PcntWithView.ParameterName = "$PcntWithView";
            command.Parameters.Add(PcntWithView);

            var ConstrClass = command.CreateParameter();
            ConstrClass.ParameterName = "$ConstrClass";
            command.Parameters.Add(ConstrClass);

            var BldgQuality = command.CreateParameter();
            BldgQuality.ParameterName = "$BldgQuality";
            command.Parameters.Add(BldgQuality);

            var Condition = command.CreateParameter();
            Condition.ParameterName = "$Condition";
            command.Parameters.Add(Condition);

            var YrBuilt = command.CreateParameter();
            YrBuilt.ParameterName = "$YrBuilt";
            command.Parameters.Add(YrBuilt);

            var EffYr = command.CreateParameter();
            EffYr.ParameterName = "$EffYr";
            command.Parameters.Add(EffYr);

            var PcntComplete = command.CreateParameter();
            PcntComplete.ParameterName = "$PcntComplete";
            command.Parameters.Add(PcntComplete);

            var Elevators = command.CreateParameter();
            Elevators.ParameterName = "$Elevators";
            command.Parameters.Add(Elevators);

            var SectySystem = command.CreateParameter();
            SectySystem.ParameterName = "$SectySystem";
            command.Parameters.Add(SectySystem);

            var Fireplace = command.CreateParameter();
            Fireplace.ParameterName = "$Fireplace";
            command.Parameters.Add(Fireplace);

            var Laundry = command.CreateParameter();
            Laundry.ParameterName = "$Laundry";
            command.Parameters.Add(Laundry);

            var AptConversion = command.CreateParameter();
            AptConversion.ParameterName = "$AptConversion";
            command.Parameters.Add(AptConversion);

            var CondoLandType = command.CreateParameter();
            CondoLandType.ParameterName = "$CondoLandType";
            command.Parameters.Add(CondoLandType);

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

            var UnitDescr = command.CreateParameter();
            UnitDescr.ParameterName = "$UnitDescr";
            command.Parameters.Add(UnitDescr);

            var ZipCode = command.CreateParameter();
            ZipCode.ParameterName = "$ZipCode";
            command.Parameters.Add(ZipCode);

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
                ComplexType.Value = record.ComplexType;
                ComplexDescr.Value = record.ComplexDescr;
                NbrBldgs.Value = record.NbrBldgs;
                NbrStories.Value = record.NbrStories;
                NbrUnits.Value = record.NbrUnits;
                AvgUnitSize.Value = record.AvgUnitSize;
                LandPerUnit.Value = record.LandPerUnit;
                ProjectLocation.Value = record.ProjectLocation;
                ProjectAppeal.Value = record.ProjectAppeal;
                PcntWithView.Value = record.PcntWithView;
                ConstrClass.Value = record.ConstrClass;
                BldgQuality.Value = record.BldgQuality;
                Condition.Value = record.Condition;
                YrBuilt.Value = record.YrBuilt;
                EffYr.Value = record.EffYr;
                PcntComplete.Value = record.PcntComplete;
                Elevators.Value = record.Elevators;
                SectySystem.Value = record.SectySystem;
                Fireplace.Value = record.Fireplace;
                Laundry.Value = record.Laundry;
                AptConversion.Value = record.AptConversion;
                CondoLandType.Value = record.CondoLandType;
                Address.Value = record.Address;
                BuildingNumber.Value = record.BuildingNumber;
                Fraction.Value = string.IsNullOrWhiteSpace(record?.Fraction) ? DBNull.Value : record.Fraction;
                DirectionPrefix.Value = record.DirectionPrefix;
                StreetName.Value = record.StreetName;
                StreetType.Value = record.StreetType;
                DirectionSuffix.Value = string.IsNullOrWhiteSpace(record?.DirectionSuffix) ? DBNull.Value : record.DirectionSuffix;
                ZipCode.Value = string.IsNullOrWhiteSpace(record?.ZipCode) ? DBNull.Value : record.ZipCode;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ComplexType = GetComplexType();
            ProjectLocation = GetProjectLocation();
            ProjectAppeal = GetProjectAppeal();
            ConstrClass = GetConstructionClass();
            BldgQuality = GetBuildingQuality();
            Condition = GetCondition();
            Laundry = GetLaundry();
            CondoLandType = GetLandType();
            Elevators = GetElevator();
            SectySystem = GetSecuritySystem();
            Fireplace = GetFireplace();
            AptConversion = GetAptConversion();
            return true;
        }

        public string GetElevator()
        {
            return Elevators switch
            {
                "Y" => "Yes",
                _ => "No",
            };
        }

        public string GetSecuritySystem()
        {
            return SectySystem switch
            {
                "Y" => "Yes",
                _ => "No",
            };
        }

        public string GetFireplace()
        {
            return Fireplace switch
            {
                "Y" => "Yes",
                _ => "No",
            };
        }

        public string GetAptConversion()
        {
            return AptConversion switch
            {
                "Y" => "Yes",
                _ => "No",
            };
        }

        public string GetLandType()
        {
            return CondoLandType switch
            {
                "1" => "Fee Simple",
                "2" => "Leased Land",
                "3" => "Air Rights",
                "4" => "Land Only",
                "5" => "Bldg Only",
                _ => string.Empty,
            };
        }

        public string GetLaundry()
        {
            return Laundry switch
            {
                "1" => "Poor",
                "2" => "Fair",
                "3" => "Average",
                "4" => "Good",
                "5" => "Very Good",
                _ => string.Empty,
            };
        }

        public string GetCondition()
        {
            return Condition switch
            {
                "1" => "Poor",
                "2" => "Fair",
                "3" => "Average",
                "4" => "Good",
                "5" => "Very Good",
                _ => string.Empty,
            };
        }

        public string GetBuildingQuality()
        {
            return BldgQuality switch
            {
                "2" => "LOW COST",
                "3" => "LOW/AVERAGE",
                "4" => "AVERAGE",
                "5" => "AVERAGE/GOOD",
                "6" => "GOOD",
                "7" => "GOOD/EXCELLENT",
                "8" => "EXCELLENT",
                _ => string.Empty,
            };
        }

        public string GetComplexType()
        {
            return ComplexType switch
            {
                "1" => "Condo,Residential",
                "2" => "Condo,Residential(Apt Use)",
                "3" => "Condo,Commercial",
                "4" => "Condo,Residential+Commercial",
                "5" => "Condo,Residential(Apt Use)+Commercial",
                "6" => "Condo,Mobile Home",
                "7" => "Condo,Floating Home",
                "8" => "Commercial",
                "9" => "Condo,Residential+Residential(Apt Use)",
                _ => string.Empty
            };
        }

        public string GetProjectLocation()
        {
            return ProjectLocation switch
            {
                "1" => "SUBSTANDARD",
                "2" => "BELOW AVERAGE",
                "3" => "AVERAGE",
                "4" => "ABOVE AVERAGE",
                "5" => "EXCELLENT",
                _ => string.Empty,
            };
        }

        public string GetProjectAppeal()
        {
            return ProjectAppeal switch
            {
                "1" => "SUBSTANDARD",
                "2" => "BELOW AVERAGE",
                "3" => "AVERAGE",
                "4" => "ABOVE AVERAGE",
                "5" => "EXCELLENT",
                _ => string.Empty,
            };
        }

        public string GetConstructionClass()
        {
            return ConstrClass switch
            {
                "1" => "STRUCTURAL STEEL",
                "2" => "REINFORCED CONCRETE",
                "3" => "MASONRY",
                "4" => "WOOD FRAME",
                "5" => "PREFAB STEEL",
                _ => string.Empty,
            };
        }
    }
    public class CondoUnit
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        [Ignore]
        public string ParcelNumber { get; set; }
        public string UnitType { get; set; }
        public string BldgNbr { get; set; }
        public string UnitNbr { get; set; }
        public string PcntOwnership { get; set; }
        public string UnitQuality { get; set; }
        public string UnitLoc { get; set; }
        public string FloorNbr { get; set; }
        public string TopFloor { get; set; }
        public string UnitOfMeasure { get; set; }
        public int Footage { get; set; }
        public string NbrBedrooms { get; set; }
        public int BathFullCount { get; set; }
        public int BathHalfCount { get; set; }
        public int Bath3qtrCount { get; set; }
        public string Fireplace { get; set; }
        public string EndUnit { get; set; }
        public string Condition { get; set; }
        public string OtherRoom { get; set; }
        public string ViewMountain { get; set; }
        public string ViewLakeRiver { get; set; }
        public string ViewCityTerritorial { get; set; }
        public string ViewPugetSound { get; set; }
        public string ViewLakeWaSamm { get; set; }
        public int PkgOpen { get; set; }
        public int PkgCarport { get; set; }
        public int PkgBasement { get; set; }
        public int PkgBasementTandem { get; set; }
        public int PkgGarage { get; set; }
        public int PkgGarageTandem { get; set; }
        public string PkgOtherType { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int YrBuilt { get; set; }
        public string Grade { get; set; }
        public string MHomeDescr { get; set; }
        public string PersPropAcctNbr { get; set; }
        public string Address { get; set; }
        public string BuildingNumber { get; set; }
        public string Fraction { get; set; }
        public string DirectionPrefix { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string DirectionSuffix { get; set; }
        public string UnitDescr { get; set; }
        public string ZipCode { get; set; }
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
                MissingFieldFound = null,
                CacheFields = true,
                BadDataFound = null,
                TrimOptions = TrimOptions.InsideQuotes
            };

            using var transaction = await context.Database.BeginTransactionAsync();
            using var reader = new StreamReader(pathToCSV);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecordsAsync<CondoUnit>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into CondoUnits (Id, Major, Minor, ParcelNumber, UnitType, BldgNbr, UnitNbr, PcntOwnership, UnitQuality, UnitLoc, FloorNbr, TopFloor, UnitOfMeasure, Footage, NbrBedrooms, BathFullCount, BathHalfCount, Bath3qtrCount, Fireplace, EndUnit, Condition, OtherRoom, ViewMountain, ViewLakeRiver, ViewCityTerritorial, ViewPugetSound, ViewLakeWaSamm, PkgOpen, PkgCarport, PkgBasement, PkgBasementTandem, PkgGarage, PkgGarageTandem, PkgOtherType, Length, Width, YrBuilt, Grade, MHomeDescr, PersPropAcctNbr, Address, BuildingNumber, Fraction, DirectionPrefix, StreetName, StreetType, DirectionSuffix, UnitDescr, ZipCode, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $UnitType, $BldgNbr, $UnitNbr, $PcntOwnership, $UnitQuality, $UnitLoc, $FloorNbr, $TopFloor, $UnitOfMeasure, $Footage, $NbrBedrooms, $BathFullCount, $BathHalfCount, $Bath3qtrCount, $Fireplace, $EndUnit, $Condition, $OtherRoom, $ViewMountain, $ViewLakeRiver, $ViewCityTerritorial, $ViewPugetSound, $ViewLakeWaSamm, $PkgOpen, $PkgCarport, $PkgBasement, $PkgBasementTandem, $PkgGarage, $PkgGarageTandem, $PkgOtherType, $Length, $Width, $YrBuilt, $Grade, $MHomeDescr, $PersPropAcctNbr, $Address, $BuildingNumber, $Fraction, $DirectionPrefix, $StreetName, $StreetType, $DirectionSuffix, $UnitDescr, $ZipCode, $IngestedOn);";

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

            var UnitType = command.CreateParameter();
            UnitType.ParameterName = "$UnitType";
            command.Parameters.Add(UnitType);

            var BldgNbr = command.CreateParameter();
            BldgNbr.ParameterName = "$BldgNbr";
            command.Parameters.Add(BldgNbr);

            var UnitNbr = command.CreateParameter();
            UnitNbr.ParameterName = "$UnitNbr";
            command.Parameters.Add(UnitNbr);

            var PcntOwnership = command.CreateParameter();
            PcntOwnership.ParameterName = "$PcntOwnership";
            command.Parameters.Add(PcntOwnership);

            var UnitQuality = command.CreateParameter();
            UnitQuality.ParameterName = "$UnitQuality";
            command.Parameters.Add(UnitQuality);

            var UnitLoc = command.CreateParameter();
            UnitLoc.ParameterName = "$UnitLoc";
            command.Parameters.Add(UnitLoc);

            var FloorNbr = command.CreateParameter();
            FloorNbr.ParameterName = "$FloorNbr";
            command.Parameters.Add(FloorNbr);

            var TopFloor = command.CreateParameter();
            TopFloor.ParameterName = "$TopFloor";
            command.Parameters.Add(TopFloor);

            var UnitOfMeasure = command.CreateParameter();
            UnitOfMeasure.ParameterName = "$UnitOfMeasure";
            command.Parameters.Add(UnitOfMeasure);

            var Footage = command.CreateParameter();
            Footage.ParameterName = "$Footage";
            command.Parameters.Add(Footage);

            var NbrBedrooms = command.CreateParameter();
            NbrBedrooms.ParameterName = "$NbrBedrooms";
            command.Parameters.Add(NbrBedrooms);

            var BathFullCount = command.CreateParameter();
            BathFullCount.ParameterName = "$BathFullCount";
            command.Parameters.Add(BathFullCount);

            var BathHalfCount = command.CreateParameter();
            BathHalfCount.ParameterName = "$BathHalfCount";
            command.Parameters.Add(BathHalfCount);

            var Bath3qtrCount = command.CreateParameter();
            Bath3qtrCount.ParameterName = "$Bath3qtrCount";
            command.Parameters.Add(Bath3qtrCount);

            var Fireplace = command.CreateParameter();
            Fireplace.ParameterName = "$Fireplace";
            command.Parameters.Add(Fireplace);

            var EndUnit = command.CreateParameter();
            EndUnit.ParameterName = "$EndUnit";
            command.Parameters.Add(EndUnit);

            var Condition = command.CreateParameter();
            Condition.ParameterName = "$Condition";
            command.Parameters.Add(Condition);

            var OtherRoom = command.CreateParameter();
            OtherRoom.ParameterName = "$OtherRoom";
            command.Parameters.Add(OtherRoom);

            var ViewMountain = command.CreateParameter();
            ViewMountain.ParameterName = "$ViewMountain";
            command.Parameters.Add(ViewMountain);

            var ViewLakeRiver = command.CreateParameter();
            ViewLakeRiver.ParameterName = "$ViewLakeRiver";
            command.Parameters.Add(ViewLakeRiver);

            var ViewCityTerritorial = command.CreateParameter();
            ViewCityTerritorial.ParameterName = "$ViewCityTerritorial";
            command.Parameters.Add(ViewCityTerritorial);

            var ViewPugetSound = command.CreateParameter();
            ViewPugetSound.ParameterName = "$ViewPugetSound";
            command.Parameters.Add(ViewPugetSound);

            var ViewLakeWaSamm = command.CreateParameter();
            ViewLakeWaSamm.ParameterName = "$ViewLakeWaSamm";
            command.Parameters.Add(ViewLakeWaSamm);

            var PkgOpen = command.CreateParameter();
            PkgOpen.ParameterName = "$PkgOpen";
            command.Parameters.Add(PkgOpen);

            var PkgCarport = command.CreateParameter();
            PkgCarport.ParameterName = "$PkgCarport";
            command.Parameters.Add(PkgCarport);

            var PkgBasement = command.CreateParameter();
            PkgBasement.ParameterName = "$PkgBasement";
            command.Parameters.Add(PkgBasement);

            var PkgBasementTandem = command.CreateParameter();
            PkgBasementTandem.ParameterName = "$PkgBasementTandem";
            command.Parameters.Add(PkgBasementTandem);

            var PkgGarage = command.CreateParameter();
            PkgGarage.ParameterName = "$PkgGarage";
            command.Parameters.Add(PkgGarage);

            var PkgGarageTandem = command.CreateParameter();
            PkgGarageTandem.ParameterName = "$PkgGarageTandem";
            command.Parameters.Add(PkgGarageTandem);

            var PkgOtherType = command.CreateParameter();
            PkgOtherType.ParameterName = "$PkgOtherType";
            command.Parameters.Add(PkgOtherType);

            var Length = command.CreateParameter();
            Length.ParameterName = "$Length";
            command.Parameters.Add(Length);

            var Width = command.CreateParameter();
            Width.ParameterName = "$Width";
            command.Parameters.Add(Width);

            var YrBuilt = command.CreateParameter();
            YrBuilt.ParameterName = "$YrBuilt";
            command.Parameters.Add(YrBuilt);

            var Grade = command.CreateParameter();
            Grade.ParameterName = "$Grade";
            command.Parameters.Add(Grade);

            var MHomeDescr = command.CreateParameter();
            MHomeDescr.ParameterName = "$MHomeDescr";
            command.Parameters.Add(MHomeDescr);

            var PersPropAcctNbr = command.CreateParameter();
            PersPropAcctNbr.ParameterName = "$PersPropAcctNbr";
            command.Parameters.Add(PersPropAcctNbr);

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

            var UnitDescr = command.CreateParameter();
            UnitDescr.ParameterName = "$UnitDescr";
            command.Parameters.Add(UnitDescr);

            var ZipCode = command.CreateParameter();
            ZipCode.ParameterName = "$ZipCode";
            command.Parameters.Add(ZipCode);

            var IngestedOn = command.CreateParameter();
            IngestedOn.ParameterName = "$IngestedOn";
            command.Parameters.Add(IngestedOn);

            await foreach (var record in records)
            {
                record.Id = Guid.NewGuid();
                record.IngestedOn = DateTime.Now;
                record.TranslateFieldsUsingLookupsToText();

                Id.Value = record.Id;
                Major.Value = string.IsNullOrWhiteSpace(record?.Major) ? DBNull.Value : record.Major;
                Minor.Value = string.IsNullOrWhiteSpace(record?.Minor) ? DBNull.Value : record.Minor;
                ParcelNumber.Value = string.IsNullOrWhiteSpace(record?.ParcelNumber) ? DBNull.Value : record.ParcelNumber;
                UnitType.Value = string.IsNullOrWhiteSpace(record?.UnitType) ? DBNull.Value : record.UnitType;
                BldgNbr.Value = string.IsNullOrWhiteSpace(record?.BldgNbr) ? DBNull.Value : record.BldgNbr;
                UnitNbr.Value = string.IsNullOrWhiteSpace(record?.UnitNbr) ? DBNull.Value : record.UnitNbr;
                PcntOwnership.Value = string.IsNullOrWhiteSpace(record?.PcntOwnership) ? DBNull.Value : record.PcntOwnership;
                UnitQuality.Value = string.IsNullOrWhiteSpace(record?.UnitQuality) ? DBNull.Value : record.UnitQuality;
                UnitLoc.Value = string.IsNullOrWhiteSpace(record?.UnitLoc) ? DBNull.Value : record.UnitLoc;
                FloorNbr.Value = string.IsNullOrWhiteSpace(record?.FloorNbr) ? DBNull.Value : record.FloorNbr;
                TopFloor.Value = string.IsNullOrWhiteSpace(record?.TopFloor) ? DBNull.Value : record.TopFloor;
                UnitOfMeasure.Value = string.IsNullOrWhiteSpace(record?.UnitOfMeasure) ? DBNull.Value : record.UnitOfMeasure;
                Footage.Value = record.Footage;
                NbrBedrooms.Value = string.IsNullOrWhiteSpace(record?.NbrBedrooms) ? DBNull.Value : record.NbrBedrooms;
                BathFullCount.Value = record.BathFullCount;
                BathHalfCount.Value = record.BathHalfCount;
                Bath3qtrCount.Value = record.Bath3qtrCount;
                Fireplace.Value = string.IsNullOrWhiteSpace(record?.Fireplace) ? DBNull.Value : record.Fireplace;
                EndUnit.Value = string.IsNullOrWhiteSpace(record?.EndUnit) ? DBNull.Value : record.EndUnit;
                Condition.Value = string.IsNullOrWhiteSpace(record?.Condition) ? DBNull.Value : record.Condition;
                OtherRoom.Value = string.IsNullOrWhiteSpace(record?.OtherRoom) ? DBNull.Value : record.OtherRoom;
                ViewMountain.Value = string.IsNullOrWhiteSpace(record?.ViewMountain) ? DBNull.Value : record.ViewMountain;
                ViewLakeRiver.Value = string.IsNullOrWhiteSpace(record?.ViewLakeRiver) ? DBNull.Value : record.ViewLakeRiver;
                ViewCityTerritorial.Value = string.IsNullOrWhiteSpace(record?.ViewCityTerritorial) ? DBNull.Value : record.ViewCityTerritorial;
                ViewPugetSound.Value = string.IsNullOrWhiteSpace(record?.ViewPugetSound) ? DBNull.Value : record.ViewPugetSound;
                ViewLakeWaSamm.Value = string.IsNullOrWhiteSpace(record?.ViewLakeWaSamm) ? DBNull.Value : record.ViewLakeWaSamm;
                PkgOpen.Value = record.PkgOpen;
                PkgCarport.Value = record.PkgCarport;
                PkgBasement.Value = record.PkgBasement;
                PkgBasementTandem.Value = record.PkgBasementTandem;
                PkgGarage.Value = record.PkgGarage;
                PkgGarageTandem.Value = record.PkgGarageTandem;
                PkgOtherType.Value = string.IsNullOrWhiteSpace(record?.PkgOtherType) ? DBNull.Value : record.PkgOtherType;
                Length.Value = record?.Length ?? 0;
                Width.Value = record?.Width ?? 0;
                YrBuilt.Value = record?.YrBuilt ?? 0;
                Grade.Value = string.IsNullOrWhiteSpace(record?.Grade) ? DBNull.Value : record.Grade;
                MHomeDescr.Value = string.IsNullOrWhiteSpace(record?.MHomeDescr) ? DBNull.Value : record.MHomeDescr;
                PersPropAcctNbr.Value = string.IsNullOrWhiteSpace(record?.PersPropAcctNbr) ? DBNull.Value : record.PersPropAcctNbr;
                Address.Value = string.IsNullOrWhiteSpace(record?.Address) ? DBNull.Value : record.Address;
                BuildingNumber.Value = string.IsNullOrWhiteSpace(record?.BuildingNumber) ? DBNull.Value : record.BuildingNumber;
                Fraction.Value = string.IsNullOrWhiteSpace(record?.Fraction) ? DBNull.Value : record.Fraction;
                DirectionPrefix.Value = string.IsNullOrWhiteSpace(record?.DirectionPrefix) ? DBNull.Value : record.DirectionPrefix;
                StreetName.Value = string.IsNullOrWhiteSpace(record?.StreetName) ? DBNull.Value : record.StreetName;
                StreetType.Value = string.IsNullOrWhiteSpace(record?.StreetType) ? DBNull.Value : record.StreetType;
                DirectionSuffix.Value = string.IsNullOrWhiteSpace(record?.DirectionSuffix) ? DBNull.Value : record.DirectionSuffix;
                UnitDescr.Value = string.IsNullOrWhiteSpace(record?.UnitDescr) ? DBNull.Value : record.UnitDescr;
                ZipCode.Value = string.IsNullOrWhiteSpace(record?.ZipCode) ? DBNull.Value : record.ZipCode;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            UnitType = GetUnitType();
            UnitQuality = GetUnitQuality();
            UnitLoc = GetUnitLocation();
            Condition = GetCondition();
            UnitOfMeasure = GetUnitOfMeasure();
            OtherRoom = GetOtherRoom();
            ViewCityTerritorial = GetViewCityTerritorial();
            ViewLakeRiver = GetViewLakeRiver();
            ViewLakeWaSamm = GetViewLakeWaSamm();
            ViewMountain = GetViewMountain();
            ViewPugetSound = GetViewPugetSound();
            PkgOtherType = GetOtherParkingType();
            Grade = GetGrade();
            TopFloor = GetTopFloor();
            EndUnit = GetEndUnit();
            Fireplace = GetFireplace();
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

        public string GetEndUnit()
        {
            return EndUnit switch
            {
                "Y" => "Yes",
                _ => "No",
            };
        }

        public string GetFireplace()
        {
            return Fireplace switch
            {
                "Y" => "Yes",
                _ => "No",
            };
        }

        public string GetTopFloor()
        {
            return TopFloor switch
            {
                "Y" => "Yes",
                _ => "No",
            };
        }

        public string GetGrade()
        {
            return Grade switch
            {
                "1" => "Poor",
                "2" => "Fair",
                "3" => "Average",
                "4" => "Good",
                "5" => "Very Good",
                "6" => "Excellent",
                _ => string.Empty
            };
        }

        public string GetOtherParkingType()
        {
            return PkgOtherType switch
            {
                "1" => "Hydraulic",
                "2" => "Other",
                _ => string.Empty
            };
        }

        public string GetViewCityTerritorial()
        {
            return ViewCityTerritorial switch
            {
                "1" => "Fair",
                "2" => "Average",
                "3" => "Good",
                "4" => "Excellent",
                _ => string.Empty,
            };
        }

        public string GetViewLakeRiver()
        {
            return ViewLakeRiver switch
            {
                "1" => "Fair",
                "2" => "Average",
                "3" => "Good",
                "4" => "Excellent",
                _ => string.Empty,
            };
        }

        public string GetViewLakeWaSamm()
        {
            return ViewLakeWaSamm switch
            {
                "1" => "Fair",
                "2" => "Average",
                "3" => "Good",
                "4" => "Excellent",
                _ => string.Empty,
            };
        }

        public string GetViewMountain()
        {
            return ViewMountain switch
            {
                "1" => "Fair",
                "2" => "Average",
                "3" => "Good",
                "4" => "Excellent",
                _ => string.Empty,
            };
        }

        public string GetViewPugetSound()
        {
            return ViewPugetSound switch
            {
                "1" => "Fair",
                "2" => "Average",
                "3" => "Good",
                "4" => "Excellent",
                _ => string.Empty,
            };
        }

        public string GetOtherRoom()
        {
            return OtherRoom switch
            {
                "1" => "Den",
                "2" => "Loft",
                _ => string.Empty
            };
        }

        public string GetUnitOfMeasure()
        {
            return UnitOfMeasure switch
            {
                "1" => "SqFt",
                "2" => "LinearFt",
                _ => string.Empty
            };
        }

        public string GetCondition()
        {
            return Condition switch
            {
                "1" => "Fair",
                "2" => "Average",
                "3" => "Good",
                "4" => "Excellent",
                _ => string.Empty,
            };
        }

        public string GetUnitLocation()
        {
            return UnitLoc switch
            {
                "1" => "Fair",
                "2" => "Average",
                "3" => "Good",
                "4" => "Excellent",
                _ => string.Empty,
            };
        }

        public string GetUnitQuality()
        {
            return UnitQuality switch
            {
                "1" => "Fair",
                "2" => "Average",
                "3" => "Good",
                "4" => "Excellent",
                _ => string.Empty,
            };
        }

        public string GetUnitType()
        {
            return UnitType switch
            {
                "1" => "Flat",
                "10" => "Leased Land",
                "11" => "Development Rights",
                "12" => "Unassigned Parking",
                "13" => "Unassigned Storage",
                "14" => "Unassigned Moorage, Open",
                "15" => "Floating Home, Flat",
                "16" => "Mobile Home",
                "17" => "Marina",
                "18" => "Hotel",
                "19" => "Warehouse",
                "2" => "Townhouse",
                "20" => "Hangar",
                "21" => "Retail",
                "22" => "Office",
                "23" => "Other Commercial",
                "24" => "Moorage, Covered",
                "25" => "Unassigned Moorage, Covered",
                "26" => "Floating Home, Townhouse",
                "27" => "Live/Work",
                "28" => "Apartments",
                "29" => "Detached SFR",
                "3" => "Penthouse,Flat",
                "4" => "Penthouse,Townhouse",
                "5" => "Parking",
                "6" => "Storage",
                "7" => "Dock",
                "8" => "Moorage, Open",
                "9" => "Land Only",
                _ => string.Empty,
            };
        }
    }
}
