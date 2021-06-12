using CsvHelper;
using CsvHelper.Configuration;

using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace eRealProperty.Models
{

    public class PropertyParcel
    {
        [Key]
        public Guid Id { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        public string ParcelNumber { get; set; }
        public string PropName { get; set; }
        public string PlatName { get; set; }
        public string PlatLot { get; set; }
        public string PlatBlock { get; set; }
        public int Range { get; set; }
        public int Township { get; set; }
        public int Section { get; set; }
        public string QuarterSection { get; set; }
        public string PropType { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
        public string SpecArea { get; set; }
        public string SpecSubArea { get; set; }
        public string DistrictName { get; set; }
        public string LevyCode { get; set; }
        public string CurrentZoning { get; set; }
        public string HBUAsIfVacant { get; set; }
        public string HBUAsImproved { get; set; }
        public string PresentUse { get; set; }
        public string SqFtLot { get; set; }
        public string WaterSystem { get; set; }
        public string SewerSystem { get; set; }
        public string Access { get; set; }
        public string Topography { get; set; }
        public string StreetSurface { get; set; }
        public string RestrictiveSzShape { get; set; }
        public string InadequateParking { get; set; }
        public string PcntUnusable { get; set; }
        public string Unbuildable { get; set; }
        public string MtRainier { get; set; }
        public string Olympics { get; set; }
        public string Cascades { get; set; }
        public string Territorial { get; set; }
        public string SeattleSkyline { get; set; }
        public string PugetSound { get; set; }
        public string LakeWashington { get; set; }
        public string LakeSammamish { get; set; }
        public string SmallLakeRiverCreek { get; set; }
        public string OtherView { get; set; }
        public string WfntLocation { get; set; }
        public string WfntFootage { get; set; }
        public string WfntBank { get; set; }
        public string WfntPoorQuality { get; set; }
        public string WfntRestrictedAccess { get; set; }
        public string WfntAccessRights { get; set; }
        public string WfntProximityInfluence { get; set; }
        public string TidelandShoreland { get; set; }
        public string LotDepthFactor { get; set; }
        public string TrafficNoise { get; set; }
        public string AirportNoise { get; set; }
        public string PowerLines { get; set; }
        public string OtherNuisances { get; set; }
        public string NbrBldgSites { get; set; }
        public string Contamination { get; set; }
        public string DNRLease { get; set; }
        public string AdjacentGolfFairway { get; set; }
        public string AdjacentGreenbelt { get; set; }
        public string HistoricSite { get; set; }
        public string CurrentUseDesignation { get; set; }
        public string NativeGrowthProtEsmt { get; set; }
        public string Easements { get; set; }
        public string OtherDesignation { get; set; }
        public string DeedRestrictions { get; set; }
        public string DevelopmentRightsPurch { get; set; }
        public string CoalMineHazard { get; set; }
        public string CriticalDrainage { get; set; }
        public string ErosionHazard { get; set; }
        public string LandfillBuffer { get; set; }
        public string HundredYrFloodPlain { get; set; }
        public string SeismicHazard { get; set; }
        public string LandslideHazard { get; set; }
        public string SteepSlopeHazard { get; set; }
        public string Stream { get; set; }
        public string Wetland { get; set; }
        public string SpeciesOfConcern { get; set; }
        public string SensitiveAreaTract { get; set; }
        public string WaterProblems { get; set; }
        public string TranspConcurrency { get; set; }
        public string OtherProblems { get; set; }
        public DateTime IngestedOn { get; set; }

        public static async Task<bool> IngestAsync(DbContextOptions<eRealPropertyContext> contextOptions)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_Parcel.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(pathToCSV))
            using (var csv = new CsvReader(reader, config))
            {
                var context = new eRealPropertyContext(contextOptions);
                context.ChangeTracker.AutoDetectChangesEnabled = false;

                var count = 0;
                const int batchSize = 10000;
                csv.Read();
                csv.ReadHeader();
                var record = new PropertyParcel();

                while (csv.Read())
                {
                    record = csv.GetRecord<PropertyParcel>();
                    record.Id = Guid.NewGuid();
                    record.IngestedOn = DateTime.Now;
                    record.TranslateFieldsUsingLookupsToText();
                    count++;
                    await context.AddAsync(record);

                    if (count != 0 && count % batchSize == 0)
                    {
                        await context.SaveChangesAsync();
                        context = new eRealPropertyContext(contextOptions);
                        context.ChangeTracker.AutoDetectChangesEnabled = false;
                    }
                }
                await context.SaveChangesAsync();
            }
            return true;
        }

        public static async Task<bool> IngestByParcelNumberAsync(string parcelNumber, eRealPropertyContext context)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_Parcel.csv");

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
                var record = new PropertyParcel();

                while (csv.Read())
                {
                    record = csv.GetRecord<PropertyParcel>();

                    if ($"{record.Major}{record.Minor}" == parcelNumber)
                    {
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
