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

    public class PropertyParcel
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
        public string PropName { get; set; }
        [CsvHelper.Configuration.Attributes.Index(3)]
        public string PlatName { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public string PlatLot { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public string PlatBlock { get; set; }
        [CsvHelper.Configuration.Attributes.Index(6)]
        public int Range { get; set; }
        [CsvHelper.Configuration.Attributes.Index(7)]
        public int Township { get; set; }
        [CsvHelper.Configuration.Attributes.Index(8)]
        public int Section { get; set; }
        [CsvHelper.Configuration.Attributes.Index(9)]
        public string QuarterSection { get; set; }
        [CsvHelper.Configuration.Attributes.Index(10)]
        public string PropType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(11)]
        public string Area { get; set; }
        [CsvHelper.Configuration.Attributes.Index(12)]
        public string SubArea { get; set; }
        [CsvHelper.Configuration.Attributes.Index(13)]
        public string SpecArea { get; set; }
        [CsvHelper.Configuration.Attributes.Index(14)]
        public string SpecSubArea { get; set; }
        [CsvHelper.Configuration.Attributes.Index(15)]
        public string DistrictName { get; set; }
        [CsvHelper.Configuration.Attributes.Index(16)]
        public string LevyCode { get; set; }
        [CsvHelper.Configuration.Attributes.Index(17)]
        public string CurrentZoning { get; set; }
        [CsvHelper.Configuration.Attributes.Index(18)]
        public string HBUAsIfVacant { get; set; }
        [CsvHelper.Configuration.Attributes.Index(19)]
        public string HBUAsImproved { get; set; }
        [CsvHelper.Configuration.Attributes.Index(20)]
        public string PresentUse { get; set; }
        [CsvHelper.Configuration.Attributes.Index(21)]
        public string SqFtLot { get; set; }
        [CsvHelper.Configuration.Attributes.Index(22)]
        public string WaterSystem { get; set; }
        [CsvHelper.Configuration.Attributes.Index(23)]
        public string SewerSystem { get; set; }
        [CsvHelper.Configuration.Attributes.Index(24)]
        public string Access { get; set; }
        [CsvHelper.Configuration.Attributes.Index(25)]
        public string Topography { get; set; }
        [CsvHelper.Configuration.Attributes.Index(26)]
        public string StreetSurface { get; set; }
        [CsvHelper.Configuration.Attributes.Index(27)]
        public string RestrictiveSzShape { get; set; }
        [CsvHelper.Configuration.Attributes.Index(28)]
        public string InadequateParking { get; set; }
        [CsvHelper.Configuration.Attributes.Index(29)]
        public string PcntUnusable { get; set; }
        [CsvHelper.Configuration.Attributes.Index(30)]
        public string Unbuildable { get; set; }
        [CsvHelper.Configuration.Attributes.Index(31)]
        public string MtRainier { get; set; }
        [CsvHelper.Configuration.Attributes.Index(32)]
        public string Olympics { get; set; }
        [CsvHelper.Configuration.Attributes.Index(33)]
        public string Cascades { get; set; }
        [CsvHelper.Configuration.Attributes.Index(34)]
        public string Territorial { get; set; }
        [CsvHelper.Configuration.Attributes.Index(35)]
        public string SeattleSkyline { get; set; }
        [CsvHelper.Configuration.Attributes.Index(36)]
        public string PugetSound { get; set; }
        [CsvHelper.Configuration.Attributes.Index(37)]
        public string LakeWashington { get; set; }
        [CsvHelper.Configuration.Attributes.Index(38)]
        public string LakeSammamish { get; set; }
        [CsvHelper.Configuration.Attributes.Index(39)]
        public string SmallLakeRiverCreek { get; set; }
        [CsvHelper.Configuration.Attributes.Index(40)]
        public string OtherView { get; set; }
        [CsvHelper.Configuration.Attributes.Index(41)]
        public string WfntLocation { get; set; }
        [CsvHelper.Configuration.Attributes.Index(42)]
        public string WfntFootage { get; set; }
        [CsvHelper.Configuration.Attributes.Index(43)]
        public string WfntBank { get; set; }
        [CsvHelper.Configuration.Attributes.Index(44)]
        public string WfntPoorQuality { get; set; }
        [CsvHelper.Configuration.Attributes.Index(45)]
        public string WfntRestrictedAccess { get; set; }
        [CsvHelper.Configuration.Attributes.Index(46)]
        public string WfntAccessRights { get; set; }
        [CsvHelper.Configuration.Attributes.Index(47)]
        public string WfntProximityInfluence { get; set; }
        [CsvHelper.Configuration.Attributes.Index(48)]
        public string TidelandShoreland { get; set; }
        [CsvHelper.Configuration.Attributes.Index(49)]
        public string LotDepthFactor { get; set; }
        [CsvHelper.Configuration.Attributes.Index(50)]
        public string TrafficNoise { get; set; }
        [CsvHelper.Configuration.Attributes.Index(51)]
        public string AirportNoise { get; set; }
        [CsvHelper.Configuration.Attributes.Index(52)]
        public string PowerLines { get; set; }
        [CsvHelper.Configuration.Attributes.Index(53)]
        public string OtherNuisances { get; set; }
        [CsvHelper.Configuration.Attributes.Index(54)]
        public string NbrBldgSites { get; set; }
        [CsvHelper.Configuration.Attributes.Index(55)]
        public string Contamination { get; set; }
        [CsvHelper.Configuration.Attributes.Index(56)]
        public string DNRLease { get; set; }
        [CsvHelper.Configuration.Attributes.Index(57)]
        public string AdjacentGolfFairway { get; set; }
        [CsvHelper.Configuration.Attributes.Index(58)]
        public string AdjacentGreenbelt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(59)]
        public string HistoricSite { get; set; }
        [CsvHelper.Configuration.Attributes.Index(60)]
        public string CurrentUseDesignation { get; set; }
        [CsvHelper.Configuration.Attributes.Index(61)]
        public string NativeGrowthProtEsmt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(62)]
        public string Easements { get; set; }
        [CsvHelper.Configuration.Attributes.Index(63)]
        public string OtherDesignation { get; set; }
        [CsvHelper.Configuration.Attributes.Index(64)]
        public string DeedRestrictions { get; set; }
        [CsvHelper.Configuration.Attributes.Index(65)]
        public string DevelopmentRightsPurch { get; set; }
        [CsvHelper.Configuration.Attributes.Index(66)]
        public string CoalMineHazard { get; set; }
        [CsvHelper.Configuration.Attributes.Index(67)]
        public string CriticalDrainage { get; set; }
        [CsvHelper.Configuration.Attributes.Index(68)]
        public string ErosionHazard { get; set; }
        [CsvHelper.Configuration.Attributes.Index(69)]
        public string LandfillBuffer { get; set; }
        [CsvHelper.Configuration.Attributes.Index(70)]
        public string HundredYrFloodPlain { get; set; }
        [CsvHelper.Configuration.Attributes.Index(71)]
        public string SeismicHazard { get; set; }
        [CsvHelper.Configuration.Attributes.Index(72)]
        public string LandslideHazard { get; set; }
        [CsvHelper.Configuration.Attributes.Index(73)]
        public string SteepSlopeHazard { get; set; }
        [CsvHelper.Configuration.Attributes.Index(74)]
        public string Stream { get; set; }
        [CsvHelper.Configuration.Attributes.Index(75)]
        public string Wetland { get; set; }
        [CsvHelper.Configuration.Attributes.Index(76)]
        public string SpeciesOfConcern { get; set; }
        [CsvHelper.Configuration.Attributes.Index(77)]
        public string SensitiveAreaTract { get; set; }
        [CsvHelper.Configuration.Attributes.Index(78)]
        public string WaterProblems { get; set; }
        [CsvHelper.Configuration.Attributes.Index(79)]
        public string TranspConcurrency { get; set; }
        [CsvHelper.Configuration.Attributes.Index(80)]
        public string OtherProblems { get; set; }
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

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into PropertyParcels (Id, Major, Minor, ParcelNumber, PropName, PlatName, PlatLot, PlatBlock, Range, Township, Section, QuarterSection, PropType, Area, SubArea, SpecArea, SpecSubArea, DistrictName, LevyCode, CurrentZoning, HBUAsIfVacant, HBUAsImproved, PresentUse, SqFtLot, WaterSystem, SewerSystem, Access, Topography, StreetSurface, RestrictiveSzShape, InadequateParking, PcntUnusable, Unbuildable, MtRainier, Olympics, Cascades, Territorial, SeattleSkyline, PugetSound, LakeWashington, LakeSammamish, SmallLakeRiverCreek, OtherView, WfntLocation, WfntFootage, WfntBank, WfntPoorQuality, WfntRestrictedAccess, WfntAccessRights, WfntProximityInfluence, TidelandShoreland, LotDepthFactor, TrafficNoise, AirportNoise, PowerLines, OtherNuisances, NbrBldgSites, Contamination, DNRLease, AdjacentGolfFairway, AdjacentGreenbelt, HistoricSite, CurrentUseDesignation, NativeGrowthProtEsmt, Easements, OtherDesignation, DeedRestrictions, DevelopmentRightsPurch, CoalMineHazard, CriticalDrainage, ErosionHazard, LandfillBuffer, HundredYrFloodPlain, SeismicHazard, LandslideHazard, SteepSlopeHazard, Stream, Wetland, SpeciesOfConcern, SensitiveAreaTract, WaterProblems, TranspConcurrency, OtherProblems, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $PropName, $PlatName, $PlatLot, $PlatBlock, $Range, $Township, $Section, $QuarterSection, $PropType, $Area, $SubArea, $SpecArea, $SpecSubArea, $DistrictName, $LevyCode, $CurrentZoning, $HBUAsIfVacant, $HBUAsImproved, $PresentUse, $SqFtLot, $WaterSystem, $SewerSystem, $Access, $Topography, $StreetSurface, $RestrictiveSzShape, $InadequateParking, $PcntUnusable, $Unbuildable, $MtRainier, $Olympics, $Cascades, $Territorial, $SeattleSkyline, $PugetSound, $LakeWashington, $LakeSammamish, $SmallLakeRiverCreek, $OtherView, $WfntLocation, $WfntFootage, $WfntBank, $WfntPoorQuality, $WfntRestrictedAccess, $WfntAccessRights, $WfntProximityInfluence, $TidelandShoreland, $LotDepthFactor, $TrafficNoise, $AirportNoise, $PowerLines, $OtherNuisances, $NbrBldgSites, $Contamination, $DNRLease, $AdjacentGolfFairway, $AdjacentGreenbelt, $HistoricSite, $CurrentUseDesignation, $NativeGrowthProtEsmt, $Easements, $OtherDesignation, $DeedRestrictions, $DevelopmentRightsPurch, $CoalMineHazard, $CriticalDrainage, $ErosionHazard, $LandfillBuffer, $HundredYrFloodPlain, $SeismicHazard, $LandslideHazard, $SteepSlopeHazard, $Stream, $Wetland, $SpeciesOfConcern, $SensitiveAreaTract, $WaterProblems, $TranspConcurrency, $OtherProblems, $IngestedOn);";

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

            var PropName = command.CreateParameter();
            PropName.ParameterName = "$PropName";
            command.Parameters.Add(PropName);

            var PlatName = command.CreateParameter();
            PlatName.ParameterName = "$PlatName";
            command.Parameters.Add(PlatName);

            var PlatLot = command.CreateParameter();
            PlatLot.ParameterName = "$PlatLot";
            command.Parameters.Add(PlatLot);

            var PlatBlock = command.CreateParameter();
            PlatBlock.ParameterName = "$PlatBlock";
            command.Parameters.Add(PlatBlock);

            var Range = command.CreateParameter();
            Range.ParameterName = "$Range";
            command.Parameters.Add(Range);

            var Township = command.CreateParameter();
            Township.ParameterName = "$Township";
            command.Parameters.Add(Township);

            var Section = command.CreateParameter();
            Section.ParameterName = "$Section";
            command.Parameters.Add(Section);

            var QuarterSection = command.CreateParameter();
            QuarterSection.ParameterName = "$QuarterSection";
            command.Parameters.Add(QuarterSection);

            var PropType = command.CreateParameter();
            PropType.ParameterName = "$PropType";
            command.Parameters.Add(PropType);

            var Area = command.CreateParameter();
            Area.ParameterName = "$Area";
            command.Parameters.Add(Area);

            var SubArea = command.CreateParameter();
            SubArea.ParameterName = "$SubArea";
            command.Parameters.Add(SubArea);

            var SpecArea = command.CreateParameter();
            SpecArea.ParameterName = "$SpecArea";
            command.Parameters.Add(SpecArea);

            var SpecSubArea = command.CreateParameter();
            SpecSubArea.ParameterName = "$SpecSubArea";
            command.Parameters.Add(SpecSubArea);

            var DistrictName = command.CreateParameter();
            DistrictName.ParameterName = "$DistrictName";
            command.Parameters.Add(DistrictName);

            var LevyCode = command.CreateParameter();
            LevyCode.ParameterName = "$LevyCode";
            command.Parameters.Add(LevyCode);

            var CurrentZoning = command.CreateParameter();
            CurrentZoning.ParameterName = "$CurrentZoning";
            command.Parameters.Add(CurrentZoning);

            var HBUAsIfVacant = command.CreateParameter();
            HBUAsIfVacant.ParameterName = "$HBUAsIfVacant";
            command.Parameters.Add(HBUAsIfVacant);

            var HBUAsImproved = command.CreateParameter();
            HBUAsImproved.ParameterName = "$HBUAsImproved";
            command.Parameters.Add(HBUAsImproved);

            var PresentUse = command.CreateParameter();
            PresentUse.ParameterName = "$PresentUse";
            command.Parameters.Add(PresentUse);

            var SqFtLot = command.CreateParameter();
            SqFtLot.ParameterName = "$SqFtLot";
            command.Parameters.Add(SqFtLot);

            var WaterSystem = command.CreateParameter();
            WaterSystem.ParameterName = "$WaterSystem";
            command.Parameters.Add(WaterSystem);

            var SewerSystem = command.CreateParameter();
            SewerSystem.ParameterName = "$SewerSystem";
            command.Parameters.Add(SewerSystem);

            var Access = command.CreateParameter();
            Access.ParameterName = "$Access";
            command.Parameters.Add(Access);

            var Topography = command.CreateParameter();
            Topography.ParameterName = "$Topography";
            command.Parameters.Add(Topography);

            var StreetSurface = command.CreateParameter();
            StreetSurface.ParameterName = "$StreetSurface";
            command.Parameters.Add(StreetSurface);

            var RestrictiveSzShape = command.CreateParameter();
            RestrictiveSzShape.ParameterName = "$RestrictiveSzShape";
            command.Parameters.Add(RestrictiveSzShape);

            var InadequateParking = command.CreateParameter();
            InadequateParking.ParameterName = "$InadequateParking";
            command.Parameters.Add(InadequateParking);

            var PcntUnusable = command.CreateParameter();
            PcntUnusable.ParameterName = "$PcntUnusable";
            command.Parameters.Add(PcntUnusable);

            var Unbuildable = command.CreateParameter();
            Unbuildable.ParameterName = "$Unbuildable";
            command.Parameters.Add(Unbuildable);

            var MtRainier = command.CreateParameter();
            MtRainier.ParameterName = "$MtRainier";
            command.Parameters.Add(MtRainier);

            var Olympics = command.CreateParameter();
            Olympics.ParameterName = "$Olympics";
            command.Parameters.Add(Olympics);

            var Cascades = command.CreateParameter();
            Cascades.ParameterName = "$Cascades";
            command.Parameters.Add(Cascades);

            var Territorial = command.CreateParameter();
            Territorial.ParameterName = "$Territorial";
            command.Parameters.Add(Territorial);

            var SeattleSkyline = command.CreateParameter();
            SeattleSkyline.ParameterName = "$SeattleSkyline";
            command.Parameters.Add(SeattleSkyline);

            var PugetSound = command.CreateParameter();
            PugetSound.ParameterName = "$PugetSound";
            command.Parameters.Add(PugetSound);

            var LakeWashington = command.CreateParameter();
            LakeWashington.ParameterName = "$LakeWashington";
            command.Parameters.Add(LakeWashington);

            var LakeSammamish = command.CreateParameter();
            LakeSammamish.ParameterName = "$LakeSammamish";
            command.Parameters.Add(LakeSammamish);

            var SmallLakeRiverCreek = command.CreateParameter();
            SmallLakeRiverCreek.ParameterName = "$SmallLakeRiverCreek";
            command.Parameters.Add(SmallLakeRiverCreek);

            var OtherView = command.CreateParameter();
            OtherView.ParameterName = "$OtherView";
            command.Parameters.Add(OtherView);

            var WfntLocation = command.CreateParameter();
            WfntLocation.ParameterName = "$WfntLocation";
            command.Parameters.Add(WfntLocation);

            var WfntFootage = command.CreateParameter();
            WfntFootage.ParameterName = "$WfntFootage";
            command.Parameters.Add(WfntFootage);

            var WfntBank = command.CreateParameter();
            WfntBank.ParameterName = "$WfntBank";
            command.Parameters.Add(WfntBank);

            var WfntPoorQuality = command.CreateParameter();
            WfntPoorQuality.ParameterName = "$WfntPoorQuality";
            command.Parameters.Add(WfntPoorQuality);

            var WfntRestrictedAccess = command.CreateParameter();
            WfntRestrictedAccess.ParameterName = "$WfntRestrictedAccess";
            command.Parameters.Add(WfntRestrictedAccess);

            var WfntAccessRights = command.CreateParameter();
            WfntAccessRights.ParameterName = "$WfntAccessRights";
            command.Parameters.Add(WfntAccessRights);

            var WfntProximityInfluence = command.CreateParameter();
            WfntProximityInfluence.ParameterName = "$WfntProximityInfluence";
            command.Parameters.Add(WfntProximityInfluence);

            var TidelandShoreland = command.CreateParameter();
            TidelandShoreland.ParameterName = "$TidelandShoreland";
            command.Parameters.Add(TidelandShoreland);

            var LotDepthFactor = command.CreateParameter();
            LotDepthFactor.ParameterName = "$LotDepthFactor";
            command.Parameters.Add(LotDepthFactor);

            var TrafficNoise = command.CreateParameter();
            TrafficNoise.ParameterName = "$TrafficNoise";
            command.Parameters.Add(TrafficNoise);

            var AirportNoise = command.CreateParameter();
            AirportNoise.ParameterName = "$AirportNoise";
            command.Parameters.Add(AirportNoise);

            var PowerLines = command.CreateParameter();
            PowerLines.ParameterName = "$PowerLines";
            command.Parameters.Add(PowerLines);

            var OtherNuisances = command.CreateParameter();
            OtherNuisances.ParameterName = "$OtherNuisances";
            command.Parameters.Add(OtherNuisances);

            var NbrBldgSites = command.CreateParameter();
            NbrBldgSites.ParameterName = "$NbrBldgSites";
            command.Parameters.Add(NbrBldgSites);

            var Contamination = command.CreateParameter();
            Contamination.ParameterName = "$Contamination";
            command.Parameters.Add(Contamination);

            var DNRLease = command.CreateParameter();
            DNRLease.ParameterName = "$DNRLease";
            command.Parameters.Add(DNRLease);

            var AdjacentGolfFairway = command.CreateParameter();
            AdjacentGolfFairway.ParameterName = "$AdjacentGolfFairway";
            command.Parameters.Add(AdjacentGolfFairway);

            var AdjacentGreenbelt = command.CreateParameter();
            AdjacentGreenbelt.ParameterName = "$AdjacentGreenbelt";
            command.Parameters.Add(AdjacentGreenbelt);

            var HistoricSite = command.CreateParameter();
            HistoricSite.ParameterName = "$HistoricSite";
            command.Parameters.Add(HistoricSite);

            var CurrentUseDesignation = command.CreateParameter();
            CurrentUseDesignation.ParameterName = "$CurrentUseDesignation";
            command.Parameters.Add(CurrentUseDesignation);

            var NativeGrowthProtEsmt = command.CreateParameter();
            NativeGrowthProtEsmt.ParameterName = "$NativeGrowthProtEsmt";
            command.Parameters.Add(NativeGrowthProtEsmt);

            var Easements = command.CreateParameter();
            Easements.ParameterName = "$Easements";
            command.Parameters.Add(Easements);

            var OtherDesignation = command.CreateParameter();
            OtherDesignation.ParameterName = "$OtherDesignation";
            command.Parameters.Add(OtherDesignation);

            var DeedRestrictions = command.CreateParameter();
            DeedRestrictions.ParameterName = "$DeedRestrictions";
            command.Parameters.Add(DeedRestrictions);

            var DevelopmentRightsPurch = command.CreateParameter();
            DevelopmentRightsPurch.ParameterName = "$DevelopmentRightsPurch";
            command.Parameters.Add(DevelopmentRightsPurch);

            var CoalMineHazard = command.CreateParameter();
            CoalMineHazard.ParameterName = "$CoalMineHazard";
            command.Parameters.Add(CoalMineHazard);

            var CriticalDrainage = command.CreateParameter();
            CriticalDrainage.ParameterName = "$CriticalDrainage";
            command.Parameters.Add(CriticalDrainage);

            var ErosionHazard = command.CreateParameter();
            ErosionHazard.ParameterName = "$ErosionHazard";
            command.Parameters.Add(ErosionHazard);

            var LandfillBuffer = command.CreateParameter();
            LandfillBuffer.ParameterName = "$LandfillBuffer";
            command.Parameters.Add(LandfillBuffer);

            var HundredYrFloodPlain = command.CreateParameter();
            HundredYrFloodPlain.ParameterName = "$HundredYrFloodPlain";
            command.Parameters.Add(HundredYrFloodPlain);

            var SeismicHazard = command.CreateParameter();
            SeismicHazard.ParameterName = "$SeismicHazard";
            command.Parameters.Add(SeismicHazard);

            var LandslideHazard = command.CreateParameter();
            LandslideHazard.ParameterName = "$LandslideHazard";
            command.Parameters.Add(LandslideHazard);

            var SteepSlopeHazard = command.CreateParameter();
            SteepSlopeHazard.ParameterName = "$SteepSlopeHazard";
            command.Parameters.Add(SteepSlopeHazard);

            var Stream = command.CreateParameter();
            Stream.ParameterName = "$Stream";
            command.Parameters.Add(Stream);

            var Wetland = command.CreateParameter();
            Wetland.ParameterName = "$Wetland";
            command.Parameters.Add(Wetland);

            var SpeciesOfConcern = command.CreateParameter();
            SpeciesOfConcern.ParameterName = "$SpeciesOfConcern";
            command.Parameters.Add(SpeciesOfConcern);

            var SensitiveAreaTract = command.CreateParameter();
            SensitiveAreaTract.ParameterName = "$SensitiveAreaTract";
            command.Parameters.Add(SensitiveAreaTract);

            var WaterProblems = command.CreateParameter();
            WaterProblems.ParameterName = "$WaterProblems";
            command.Parameters.Add(WaterProblems);

            var TranspConcurrency = command.CreateParameter();
            TranspConcurrency.ParameterName = "$TranspConcurrency";
            command.Parameters.Add(TranspConcurrency);

            var OtherProblems = command.CreateParameter();
            OtherProblems.ParameterName = "$OtherProblems";
            command.Parameters.Add(OtherProblems);

            var IngestedOn = command.CreateParameter();
            IngestedOn.ParameterName = "$IngestedOn";
            command.Parameters.Add(IngestedOn);

            var records = csv.GetRecordsAsync<PropertyParcel>();

            await foreach (var record in records)
            {
                record.Id = Guid.NewGuid();
                record.IngestedOn = DateTime.Now;
                record.TranslateFieldsUsingLookupsToText();

                Id.Value = record.Id;
                Major.Value = record.Major;
                Minor.Value = record.Minor;
                ParcelNumber.Value = record.ParcelNumber;
                PropName.Value = string.IsNullOrWhiteSpace(record?.PropName) ? DBNull.Value : record.PropName;
                PlatName.Value = string.IsNullOrWhiteSpace(record?.PlatName) ? DBNull.Value : record.PlatName;
                PlatLot.Value = string.IsNullOrWhiteSpace(record?.PlatLot) ? DBNull.Value : record.PlatLot;
                PlatBlock.Value = string.IsNullOrWhiteSpace(record?.PlatBlock) ? DBNull.Value : record.PlatBlock;
                Range.Value = record.Range;
                Township.Value = record.Township;
                Section.Value = record.Section;
                QuarterSection.Value = string.IsNullOrWhiteSpace(record?.QuarterSection) ? DBNull.Value : record.QuarterSection;
                PropType.Value = string.IsNullOrWhiteSpace(record?.PropType) ? DBNull.Value : record.PropType;
                Area.Value = string.IsNullOrWhiteSpace(record?.Area) ? DBNull.Value : record.Area;
                SubArea.Value = string.IsNullOrWhiteSpace(record?.SubArea) ? DBNull.Value : record.SubArea;
                SpecArea.Value = string.IsNullOrWhiteSpace(record?.SpecArea) ? DBNull.Value : record.SpecArea;
                SpecSubArea.Value = string.IsNullOrWhiteSpace(record?.SpecSubArea) ? DBNull.Value : record.SpecSubArea;
                DistrictName.Value = string.IsNullOrWhiteSpace(record?.DistrictName) ? DBNull.Value : record.DistrictName;
                LevyCode.Value = string.IsNullOrWhiteSpace(record?.LevyCode) ? DBNull.Value : record.LevyCode;
                CurrentZoning.Value = string.IsNullOrWhiteSpace(record?.CurrentZoning) ? DBNull.Value : record.CurrentZoning;
                HBUAsIfVacant.Value = string.IsNullOrWhiteSpace(record?.HBUAsIfVacant) ? DBNull.Value : record.HBUAsIfVacant;
                HBUAsImproved.Value = string.IsNullOrWhiteSpace(record?.HBUAsImproved) ? DBNull.Value : record.HBUAsImproved;
                PresentUse.Value = string.IsNullOrWhiteSpace(record?.PresentUse) ? DBNull.Value : record.PresentUse;
                SqFtLot.Value = string.IsNullOrWhiteSpace(record?.SqFtLot) ? DBNull.Value : record.SqFtLot;
                WaterSystem.Value = string.IsNullOrWhiteSpace(record?.WaterSystem) ? DBNull.Value : record.WaterSystem;
                SewerSystem.Value = string.IsNullOrWhiteSpace(record?.SewerSystem) ? DBNull.Value : record.SewerSystem;
                Access.Value = string.IsNullOrWhiteSpace(record?.Access) ? DBNull.Value : record.Access;
                Topography.Value = string.IsNullOrWhiteSpace(record?.Topography) ? DBNull.Value : record.Topography;
                StreetSurface.Value = string.IsNullOrWhiteSpace(record?.StreetSurface) ? DBNull.Value : record.StreetSurface;
                RestrictiveSzShape.Value = string.IsNullOrWhiteSpace(record.RestrictiveSzShape) ? DBNull.Value : record.RestrictiveSzShape;
                InadequateParking.Value = string.IsNullOrWhiteSpace(record?.InadequateParking) ? DBNull.Value : record.InadequateParking;
                PcntUnusable.Value = string.IsNullOrWhiteSpace(record?.PcntUnusable) ? DBNull.Value : record.PcntUnusable;
                Unbuildable.Value = string.IsNullOrWhiteSpace(record?.Unbuildable) ? DBNull.Value : record.Unbuildable;
                MtRainier.Value = string.IsNullOrWhiteSpace(record?.MtRainier) ? DBNull.Value : record.MtRainier;
                Olympics.Value = string.IsNullOrWhiteSpace(record?.Olympics) ? DBNull.Value : record.Olympics;
                Cascades.Value = string.IsNullOrWhiteSpace(record?.Cascades) ? DBNull.Value : record.Cascades;
                Territorial.Value = string.IsNullOrWhiteSpace(record?.Territorial) ? DBNull.Value : record.Territorial;
                SeattleSkyline.Value = string.IsNullOrWhiteSpace(record?.SeattleSkyline) ? DBNull.Value : record.SeattleSkyline;
                PugetSound.Value = string.IsNullOrWhiteSpace(record?.PugetSound) ? DBNull.Value : record.PugetSound;
                LakeWashington.Value = string.IsNullOrWhiteSpace(record?.LakeWashington) ? DBNull.Value : record.LakeWashington;
                LakeSammamish.Value = string.IsNullOrWhiteSpace(record?.LakeSammamish) ? DBNull.Value : record.LakeSammamish;
                SmallLakeRiverCreek.Value = string.IsNullOrWhiteSpace(record?.SmallLakeRiverCreek) ? DBNull.Value : record.SmallLakeRiverCreek;
                OtherView.Value = string.IsNullOrWhiteSpace(record?.OtherView) ? DBNull.Value : record.OtherView;
                WfntLocation.Value = string.IsNullOrWhiteSpace(record?.WfntLocation) ? DBNull.Value : record.WfntLocation;
                WfntFootage.Value = string.IsNullOrWhiteSpace(record?.WfntFootage) ? DBNull.Value : record.WfntFootage;
                WfntBank.Value = string.IsNullOrWhiteSpace(record?.WfntBank) ? DBNull.Value : record.WfntBank;
                WfntPoorQuality.Value = string.IsNullOrWhiteSpace(record?.WfntPoorQuality) ? DBNull.Value : record.WfntPoorQuality;
                WfntRestrictedAccess.Value = string.IsNullOrWhiteSpace(record?.WfntRestrictedAccess) ? DBNull.Value : record.WfntRestrictedAccess;
                WfntAccessRights.Value = string.IsNullOrWhiteSpace(record?.WfntAccessRights) ? DBNull.Value : record.WfntAccessRights;
                WfntProximityInfluence.Value = string.IsNullOrWhiteSpace(record?.WfntProximityInfluence) ? DBNull.Value : record.WfntProximityInfluence;
                TidelandShoreland.Value = string.IsNullOrWhiteSpace(record?.TidelandShoreland) ? DBNull.Value : record.TidelandShoreland;
                LotDepthFactor.Value = string.IsNullOrWhiteSpace(record?.LotDepthFactor) ? DBNull.Value : record.LotDepthFactor;
                TrafficNoise.Value = string.IsNullOrWhiteSpace(record?.TrafficNoise) ? DBNull.Value : record.TrafficNoise;
                AirportNoise.Value = string.IsNullOrWhiteSpace(record?.AirportNoise) ? DBNull.Value : record.AirportNoise;
                PowerLines.Value = string.IsNullOrWhiteSpace(record?.PowerLines) ? DBNull.Value : record.PowerLines;
                OtherNuisances.Value = string.IsNullOrWhiteSpace(record?.OtherNuisances) ? DBNull.Value : record.OtherNuisances;
                NbrBldgSites.Value = string.IsNullOrWhiteSpace(record?.NbrBldgSites) ? DBNull.Value : record.NbrBldgSites;
                Contamination.Value = string.IsNullOrWhiteSpace(record?.Contamination) ? DBNull.Value : record.Contamination;
                DNRLease.Value = string.IsNullOrWhiteSpace(record?.DNRLease) ? DBNull.Value : record.DNRLease;
                AdjacentGolfFairway.Value = string.IsNullOrWhiteSpace(record?.AdjacentGolfFairway) ? DBNull.Value : record.AdjacentGolfFairway;
                AdjacentGreenbelt.Value = string.IsNullOrWhiteSpace(record?.AdjacentGreenbelt) ? DBNull.Value : record.AdjacentGreenbelt;
                HistoricSite.Value = string.IsNullOrWhiteSpace(record?.HistoricSite) ? DBNull.Value : record.HistoricSite;
                CurrentUseDesignation.Value = string.IsNullOrWhiteSpace(record?.CurrentUseDesignation) ? DBNull.Value : record.CurrentUseDesignation;
                NativeGrowthProtEsmt.Value = string.IsNullOrWhiteSpace(record?.NativeGrowthProtEsmt) ? DBNull.Value : record.NativeGrowthProtEsmt;
                Easements.Value = string.IsNullOrWhiteSpace(record?.Easements) ? DBNull.Value : record.Easements;
                OtherDesignation.Value = string.IsNullOrWhiteSpace(record?.OtherDesignation) ? DBNull.Value : record.OtherDesignation;
                DeedRestrictions.Value = string.IsNullOrWhiteSpace(record?.DeedRestrictions) ? DBNull.Value : record.DeedRestrictions;
                DevelopmentRightsPurch.Value = string.IsNullOrWhiteSpace(record?.DevelopmentRightsPurch) ? DBNull.Value : record.DevelopmentRightsPurch;
                CoalMineHazard.Value = string.IsNullOrWhiteSpace(record?.CoalMineHazard) ? DBNull.Value : record.CoalMineHazard;
                CriticalDrainage.Value = string.IsNullOrWhiteSpace(record?.CriticalDrainage) ? DBNull.Value : record.CriticalDrainage;
                ErosionHazard.Value = string.IsNullOrWhiteSpace(record?.ErosionHazard) ? DBNull.Value : record.ErosionHazard;
                LandfillBuffer.Value = string.IsNullOrWhiteSpace(record?.LandfillBuffer) ? DBNull.Value : record.LandfillBuffer;
                HundredYrFloodPlain.Value = string.IsNullOrWhiteSpace(record?.HundredYrFloodPlain) ? DBNull.Value : record.HundredYrFloodPlain;
                SeismicHazard.Value = string.IsNullOrWhiteSpace(record?.SeismicHazard) ? DBNull.Value : record.SeismicHazard;
                LandslideHazard.Value = string.IsNullOrWhiteSpace(record?.LandslideHazard) ? DBNull.Value : record.LandslideHazard;
                SteepSlopeHazard.Value = string.IsNullOrWhiteSpace(record?.SteepSlopeHazard) ? DBNull.Value : record.SteepSlopeHazard;
                Stream.Value = string.IsNullOrWhiteSpace(record?.Stream) ? DBNull.Value : record.Stream;
                Wetland.Value = string.IsNullOrWhiteSpace(record?.Wetland) ? DBNull.Value : record.Wetland;
                SpeciesOfConcern.Value = string.IsNullOrWhiteSpace(record?.SpeciesOfConcern) ? DBNull.Value : record.SpeciesOfConcern;
                SensitiveAreaTract.Value = string.IsNullOrWhiteSpace(record?.SensitiveAreaTract) ? DBNull.Value : record.SensitiveAreaTract;
                WaterProblems.Value = string.IsNullOrWhiteSpace(record?.WaterProblems) ? DBNull.Value : record.WaterProblems;
                TranspConcurrency.Value = string.IsNullOrWhiteSpace(record?.TranspConcurrency) ? DBNull.Value : record.TranspConcurrency;
                OtherProblems.Value = string.IsNullOrWhiteSpace(record?.OtherProblems) ? DBNull.Value : record.OtherProblems;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            PropType = GetPropertyType();
            ParcelNumber = GetParcelNumber();
            WfntLocation = GetWaterfrontLocation();
            WfntBank = GetWaterfrontBank();
            WfntPoorQuality = GetWaterfrontQuality();
            WfntRestrictedAccess = GetWaterFrontRestrictedAccess();
            TidelandShoreland = GetWaterfrontTidelandOrShoreland();
            HBUAsIfVacant = GetHBUAsIfVacant();
            HBUAsImproved = GetHBUAsImproved();
            WaterSystem = GetWaterSystem();
            SewerSystem = GetSewerSystem();
            Access = GetAccess();
            StreetSurface = GetStreetSurface();
            InadequateParking = GetInadequateParking();
            PresentUse = GetPresentUse();
            TrafficNoise = GetTrafficNoise();
            HistoricSite = GetHistoricSite();
            CurrentUseDesignation = GetCurrentUseDesignation();
            // Hanlde all the random views and their ratings.
            MtRainier = GetViewRating(MtRainier);
            Olympics = GetViewRating(Olympics);
            Cascades = GetViewRating(Cascades);
            Territorial = GetViewRating(Territorial);
            SeattleSkyline = GetViewRating(SeattleSkyline);
            PugetSound = GetViewRating(PugetSound);
            LakeWashington = GetViewRating(LakeWashington);
            LakeSammamish = GetViewRating(LakeSammamish);
            SmallLakeRiverCreek = GetViewRating(SmallLakeRiverCreek);
            OtherView = GetViewRating(OtherView);

            return true;
        }

        public string GetPropertyType()
        {
            return PropType switch
            {
                "C" => "Commercial",
                "K" => "Condominium",
                "M" => "Coal & Mineral Rights",
                "N" => "Mining",
                "R" => "Residential",
                "T" => "Timber",
                "U" => "Undivided Interest",
                "X" => "Exempt",
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

        public string GetViewRating(string viewValue)
        {
            // Fancy switch statements too.
            return viewValue switch
            {
                "0" => null, // This indicates that there is no view. So we skip it.
                "1" => "Fair",
                "2" => "Average",
                "3" => "Good",
                "4" => "Excellent",
                _ => null,
            };
        }

        // Lookup 95
        public string GetTrafficNoise()
        {
            if (!string.IsNullOrWhiteSpace(TrafficNoise))
            {
                return TrafficNoise switch
                {
                    "0" => null,
                    "1" => "Moderate",
                    "2" => "High",
                    "3" => "Extreme",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 67
        public string GetHistoricSite()
        {
            if (!string.IsNullOrWhiteSpace(HistoricSite))
            {
                return HistoricSite switch
                {
                    "0" => null,
                    "1" => "District",
                    "2" => "Inventory",
                    "3" => "Designated",
                    "4" => "Vacant Historical Land",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }
        // Lookup 16
        public string GetCurrentUseDesignation()
        {
            if (!string.IsNullOrWhiteSpace(HistoricSite))
            {
                return HistoricSite switch
                {
                    "0" => null,
                    "1" => "Agricultural",
                    "2" => "Timber (RCW 84.34)",
                    "3" => "Open Space",
                    "4" => "Forest Land (RCW 84.33)",
                    // TODO: What does this mean, Can we get an english translation of this shorthand?
                    "5" => "CLFRS",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        public string GetWaterfrontLocation()
        {
            if (!string.IsNullOrWhiteSpace(WfntLocation))
            {
                return WfntLocation switch
                {
                    "0" => null,
                    "1" => "Duwamish River",
                    "2" => "Elliot Bay",
                    "3" => "Puget Sound",
                    "4" => "Lake Union",
                    "5" => "Ship Canal",
                    "6" => "Lake Washington",
                    "7" => "Lake Sammamish",
                    "8" => "Other Lake",
                    "9" => "River or Slough",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        public string GetWaterfrontBank()
        {
            if (!string.IsNullOrWhiteSpace(WfntBank))
            {
                return WfntBank switch
                {
                    "0" => null,
                    "1" => "Low Bank",
                    "2" => "Medium Bank",
                    "3" => "High Bank",
                    "4" => "No Bank",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        public string GetWaterfrontQuality()
        {
            if (!string.IsNullOrWhiteSpace(WfntPoorQuality))
            {
                if (WfntPoorQuality == "1")
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
            else
            {
                return "No";
            }
        }

        public string GetWaterFrontRestrictedAccess()
        {
            if (!string.IsNullOrWhiteSpace(WfntRestrictedAccess))
            {
                return WfntRestrictedAccess switch
                {
                    "0" => null,
                    "1" => "To Residence",
                    "2" => "To Waterfront",
                    "3" => "No Waterfront Access",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        public string GetWaterfrontTidelandOrShoreland()
        {
            if (!string.IsNullOrWhiteSpace(TidelandShoreland))
            {
                return TidelandShoreland switch
                {
                    "0" => null,
                    "1" => "Uplands Only",
                    "2" => "Uplands with Tidelands or Shorelines",
                    "3" => "Tidelands or Shorelines only",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        public string GetHBUAsIfVacant()
        {
            if (!string.IsNullOrWhiteSpace(HBUAsIfVacant))
            {
                return HBUAsIfVacant switch
                {
                    "0" => null,
                    "1" => "Single Family",
                    "2" => "Duplex",
                    "3" => "Triplex",
                    "4" => "Mobile Home",
                    "5" => "Other Single Family Dwelling",
                    "6" => "Multi-Family Dwelling",
                    "7" => "Group Residence",
                    "8" => "Temporary Lodging",
                    "9" => "Park or Recreation",
                    "10" => "Amusement or Entertainment",
                    "11" => "Cultural",
                    "12" => "Educational Service",
                    "13" => "Commercial Service",
                    "14" => "Retail or Wholesale",
                    "15" => "Manufacturing",
                    "16" => "Agricultural",
                    "17" => "Forestry",
                    "18" => "Fish and Wildlife Management",
                    "19" => "Mineral",
                    "20" => "Regional Land Use",
                    "21" => "Mixed Use",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        public string GetHBUAsImproved()
        {
            if (!string.IsNullOrWhiteSpace(HBUAsImproved))
            {
                return HBUAsImproved switch
                {
                    "0" => null,
                    "1" => "Present Use",
                    "2" => "Interim Use",
                    "3" => "Tear Down",
                    "4" => "Other",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 56
        public string GetWaterSystem()
        {
            if (!string.IsNullOrWhiteSpace(WaterSystem))
            {
                return WaterSystem switch
                {
                    "0" => null,
                    "1" => "Private",
                    "2" => "Water District",
                    "3" => "Private Restricted",
                    "4" => "Public Restricted",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 57
        public string GetSewerSystem()
        {
            if (!string.IsNullOrWhiteSpace(SewerSystem))
            {
                return SewerSystem switch
                {
                    "0" => null,
                    "1" => "Private",
                    "2" => "Water District",
                    "3" => "Private Restricted",
                    "4" => "Public Restricted",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 55
        public string GetAccess()
        {
            if (!string.IsNullOrWhiteSpace(Access))
            {
                return Access switch
                {
                    "0" => null,
                    "1" => "Restricted",
                    "2" => "Legal/Undeveloped",
                    "3" => "Private",
                    "4" => "Public",
                    "5" => "Walk In",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 60
        public string GetStreetSurface()
        {
            if (!string.IsNullOrWhiteSpace(StreetSurface))
            {
                return StreetSurface switch
                {
                    "0" => null,
                    "1" => "Paved",
                    "2" => "Gravel",
                    "3" => "Dirt",
                    "4" => "Undeveloped",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 92
        public string GetInadequateParking()
        {
            if (!string.IsNullOrWhiteSpace(InadequateParking))
            {
                return InadequateParking switch
                {
                    "0" => null,
                    "1" => "Inadequate",
                    "2" => "Adequate",
                    _ => null,
                };
            }
            else
            {
                return null;
            }
        }

        public string GetPresentUse()
        {
            if (!string.IsNullOrWhiteSpace(PresentUse))
            {
                return PresentUse switch
                {
                    "0" => null,
                    "2" => "Single Family (Residental Use or Zone)",
                    "3" => "Duplex",
                    "4" => "Triplex",
                    "5" => "4-Plex",
                    "6" => "Single Family (Commerical or Industrial Zone)",
                    "7" => "Houseboat",
                    "8" => "Mobile Home",
                    "9" => "Single Family (Commerical or Industrial Use)",
                    "10" => "Congregate Housing",
                    "11" => "Apartment",
                    "16" => "Apartment (Mixed Use)",
                    "17" => "Apartment (Co-op)",
                    "18" => "Apartment (Subsidized)",
                    "20" => "Condominium (Residential)",
                    "25" => "Condominium (Mixed Use)",
                    "29" => "Townhouse Plat",
                    "38" => "Mobile Home Park",
                    "48" => "Condominium (Moblie Home Park)",
                    "49" => "Retirement Facility",
                    "51" => "Hotel or Motel",
                    "55" => "Rehabilitation Center",
                    "56" => "Residence Hall or Dorm",
                    "57" => "Group Home",
                    "58" => "Resort or Lodge or Retreat",
                    "59" => "Nursing Home",
                    "60" => "Shopping Center (Neighborhood)",
                    "61" => "Shopping Center (Community)",
                    "62" => "Shopping Center (Regional)",
                    "63" => "Shopping Center (Major Retail)",
                    "64" => "Shopping Center (Specialty)",
                    "96" => "Retail (Line or Strip)",
                    "101" => "Retail Store",
                    "104" => "Retail (Big Box)",
                    "105" => "Retail (Discount)",
                    "106" => "Office Building",
                    "118" => "Office Park",
                    "122" => "Medical or Dental Office",
                    "126" => "Condominium (Office)",
                    "130" => "Farm",
                    "137" => "Greenhousr or Nursery or Horticutlural Services",
                    "138" => "Mining or Quarry or Ore Processing",
                    "140" => "Bowling Alley",
                    "141" => "Campground",
                    "142" => "Driving Range",
                    "143" => "Golf Course",
                    "145" => "Health Club",
                    "146" => "Marina",
                    "147" => "Movie Theater",
                    "149" => "Park, Public (Zoo or Arbor)",
                    "150" => "Park, Private (Amusement Center)",
                    "152" => "Ski Area",
                    "153" => "Skating Rink (Ice or Roller)",
                    "156" => "Sport Facility",
                    "157" => "Art Gallery or Museum or Social Services",
                    "159" => "Parking (Assoc)",
                    "160" => "Auditorium or Assembly Bldg",
                    "161" => "Auto Showroom and Lot",
                    "162" => "Bank",
                    "163" => "Car Wash",
                    "165" => "Church or Welfare or Religious Services",
                    "166" => "Club",
                    "167" => "Conv Store without Gas",
                    "168" => "Conv Store with Gas",
                    "171" => "Restaurant (Fast Food)",
                    "172" => "Governmental Service",
                    "173" => "Hospital",
                    "179" => "Mortuary or Cemetery or Crematory",
                    "180" => "Parking (Commercial Lot)",
                    "182" => "Parking (Garage)",
                    "183" => "Restaurant or Lounge",
                    "184" => "School (Public)",
                    "185" => "School (Private)",
                    "186" => "Service Station",
                    "188" => "Tavern or Lounge",
                    "189" => "Post Office or Post Service",
                    "190" => "Vet or Animal Control Service",
                    "191" => "Grocery Store",
                    "193" => "Daycare Center",
                    "194" => "Mini Lube",
                    "195" => "Warehouse",
                    "202" => "High Tech or High Flex",
                    "210" => "Industrial Park",
                    "216" => "Service Building",
                    "223" => "Industrial (Gen Purpose)",
                    "245" => "Industrial (Heavy)",
                    "246" => "Industrial (Light)",
                    "247" => "Air Terminal and Hangars",
                    "252" => "Mini Warehouse",
                    "261" => "Terminal (Rail)",
                    "262" => "Terminal (Marine or Commercial Fishing)",
                    "263" => "Terminal (Grain)",
                    "264" => "Terminal (Auto or Bus or Other)",
                    "266" => "Utility, Public",
                    "267" => "Utility, Private (Radio or T.V.)",
                    "271" => "Terminal (Marine)",
                    "272" => "Historic Property (Residence)",
                    "273" => "Historic Property (Office)",
                    "274" => "Historic Property (Retail)",
                    "275" => "Historic Property (Eat or Drink)",
                    "276" => "Historic Property (Loft or Warehouse)",
                    "277" => "Historic Property (Park or Billboard)",
                    "278" => "Historic Property (Transient Facility)",
                    "279" => "Historic Property (Recreational or Entertainment)",
                    "280" => "Historic Property (Misc)",
                    "299" => "Historic Property (Vacant Land)",
                    "300" => "Vacant (Single-family)",
                    "301" => "Vacant (Multi-family)",
                    "309" => "Vacant (Commercial)",
                    "316" => "Vacant (Industrial)",
                    "323" => "Reforestation (RCW 84.28)",
                    "324" => "Forest Land (Class-RCW 84.33)",
                    "325" => "Forest Land (Desig-RCW 84.33)",
                    "326" => "Open Space (Curr Use-RCW 84.34)",
                    "327" => "Open Space (Agric-RCW 84.34)",
                    "328" => "Open Space Timber Land or Greenbelt",
                    "330" => "Easement",
                    "331" => "Reserve or Wilderness Area",
                    "332" => "Right of Way or Utility, Road",
                    "333" => "River or Creek or Stream",
                    "334" => "Tideland, 1st Class",
                    "335" => "Tideland, 2nd Class",
                    "336" => "Transferable Dev Rights",
                    "337" => "Water Body, Fresh",
                    "339" => "Shell Structure",
                    "340" => "Bed & Breakfast",
                    "341" => "Rooming House",
                    "342" => "Fraternity or Sorority House",
                    "343" => "Gas Station",
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
