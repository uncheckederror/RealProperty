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
    public class ApartmentComplex
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
        public string ProjectLocation { get; set; }
        [CsvHelper.Configuration.Attributes.Index(8)]
        public string ProjectAppeal { get; set; }
        [CsvHelper.Configuration.Attributes.Index(9)]
        public string PcntWithView { get; set; }
        [CsvHelper.Configuration.Attributes.Index(10)]
        public string ConstrClass { get; set; }
        [CsvHelper.Configuration.Attributes.Index(11)]
        public string BldgQuality { get; set; }
        [CsvHelper.Configuration.Attributes.Index(12)]
        public string Condition { get; set; }
        [CsvHelper.Configuration.Attributes.Index(13)]
        public int YrBuilt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(14)]
        public int EffYr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(15)]
        public string PcntComplete { get; set; }
        [CsvHelper.Configuration.Attributes.Index(16)]
        public string Elevators { get; set; }
        [CsvHelper.Configuration.Attributes.Index(17)]
        public string SectySystem { get; set; }
        [CsvHelper.Configuration.Attributes.Index(18)]
        public string Fireplace { get; set; }
        [CsvHelper.Configuration.Attributes.Index(19)]
        public string Laundry { get; set; }
        [CsvHelper.Configuration.Attributes.Index(20)]
        public string Address { get; set; }
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

            var records = csv.GetRecordsAsync<ApartmentComplex>();


            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into ApartmentComplexes (Id, Major, Minor, ParcelNumber, ComplexDescr, NbrBldgs, NbrStories, NbrUnits, AvgUnitSize, ProjectLocation, ProjectAppeal, PcntWithView, ConstrClass, BldgQuality, Condition, YrBuilt, EffYr, PcntComplete, Elevators, SectySystem, Fireplace, Laundry, Address, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $ComplexDescr, $NbrBldgs, $NbrStories, $NbrUnits, $AvgUnitSize, $ProjectLocation, $ProjectAppeal, $PcntWithView, $ConstrClass, $BldgQuality, $Condition, $YrBuilt, $EffYr, $PcntComplete, $Elevators, $SectySystem, $Fireplace, $Laundry, $Address, $IngestedOn);";

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

            var Address = command.CreateParameter();
            Address.ParameterName = "$Address";
            command.Parameters.Add(Address);

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
                ComplexDescr.Value = record.ComplexDescr;
                NbrBldgs.Value = record.NbrBldgs;
                NbrStories.Value = record.NbrStories;
                NbrUnits.Value = record.NbrUnits;
                AvgUnitSize.Value = record.AvgUnitSize;
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
                Address.Value = record.Address;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            Elevators = GetElevator();
            Fireplace = GetFireplace();
            ProjectLocation = GetProjectLocation();
            ProjectAppeal = GetProjectAppeal();
            ConstrClass = GetConstructionClass();
            BldgQuality = GetBuildingQuality();
            Condition = GetCondition();
            Laundry = GetLaundry();
            return true;
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
                "4" => "AVERAGE",
                "5" => "AVERAGE/GOOD",
                "6" => "GOOD",
                "7" => "GOOD/EXCELLENT",
                "8" => "EXCELLENT",
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

        public string GetElevator()
        {
            return Elevators switch
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
    }
}
