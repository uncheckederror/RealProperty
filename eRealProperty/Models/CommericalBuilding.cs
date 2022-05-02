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
    public class CommericalBuilding
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
        public int NbrBldgs { get; set; }
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
        public int NbrStories { get; set; }
        [CsvHelper.Configuration.Attributes.Index(13)]
        public string PredominantUse { get; set; }
        [CsvHelper.Configuration.Attributes.Index(14)]
        public string Shape { get; set; }
        [CsvHelper.Configuration.Attributes.Index(15)]
        public string ConstrClass { get; set; }
        [CsvHelper.Configuration.Attributes.Index(16)]
        public string BldgQuality { get; set; }
        [CsvHelper.Configuration.Attributes.Index(17)]
        public string BldgDescr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(18)]
        public int BldgGrossSqFt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(19)]
        public int BldgNetSqFt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(20)]
        public int YrBuilt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(21)]
        public int EffYr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(22)]
        public int PcntComplete { get; set; }
        [CsvHelper.Configuration.Attributes.Index(23)]
        public string HeatingSystem { get; set; }
        [CsvHelper.Configuration.Attributes.Index(24)]
        public string Sprinklers { get; set; }
        [CsvHelper.Configuration.Attributes.Index(25)]
        public string Elevators { get; set; }
        [Ignore]
        public DateTime IngestedOn { get; set; }

        public static async Task<bool> IngestAsync(eRealPropertyContext context, string pathToCSV, CsvConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(pathToCSV) || config is null || context is null)
            {
                return false;
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            using var reader = new StreamReader(pathToCSV, config.Encoding);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecordsAsync<CommericalBuilding>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into CommericalBuildings (Id, Major, Minor, ParcelNumber, BldgNbr, NbrBldgs, Address, BuildingNumber, Fraction, DirectionPrefix, StreetName, StreetType, DirectionSuffix, ZipCode, NbrStories, PredominantUse, Shape, ConstrClass, BldgQuality, BldgDescr, BldgGrossSqFt, BldgNetSqFt, YrBuilt, EffYr, PcntComplete, HeatingSystem, Sprinklers, Elevators, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $BldgNbr, $NbrBldgs, $Address, $BuildingNumber, $Fraction, $DirectionPrefix, $StreetName, $StreetType, $DirectionSuffix, $ZipCode, $NbrStories, $PredominantUse, $Shape, $ConstrClass, $BldgQuality, $BldgDescr, $BldgGrossSqFt, $BldgNetSqFt, $YrBuilt, $EffYr, $PcntComplete, $HeatingSystem, $Sprinklers, $Elevators, $IngestedOn);";

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

            var NbrBldgs = command.CreateParameter();
            NbrBldgs.ParameterName = "$NbrBldgs";
            command.Parameters.Add(NbrBldgs);

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

            var NbrStories = command.CreateParameter();
            NbrStories.ParameterName = "$NbrStories";
            command.Parameters.Add(NbrStories);

            var PredominantUse = command.CreateParameter();
            PredominantUse.ParameterName = "$PredominantUse";
            command.Parameters.Add(PredominantUse);

            var Shape = command.CreateParameter();
            Shape.ParameterName = "$Shape";
            command.Parameters.Add(Shape);

            var ConstrClass = command.CreateParameter();
            ConstrClass.ParameterName = "$ConstrClass";
            command.Parameters.Add(ConstrClass);

            var BldgQuality = command.CreateParameter();
            BldgQuality.ParameterName = "$BldgQuality";
            command.Parameters.Add(BldgQuality);

            var BldgDescr = command.CreateParameter();
            BldgDescr.ParameterName = "$BldgDescr";
            command.Parameters.Add(BldgDescr);

            var BldgGrossSqFt = command.CreateParameter();
            BldgGrossSqFt.ParameterName = "$BldgGrossSqFt";
            command.Parameters.Add(BldgGrossSqFt);

            var BldgNetSqFt = command.CreateParameter();
            BldgNetSqFt.ParameterName = "$BldgNetSqFt";
            command.Parameters.Add(BldgNetSqFt);

            var YrBuilt = command.CreateParameter();
            YrBuilt.ParameterName = "$YrBuilt";
            command.Parameters.Add(YrBuilt);

            var EffYr = command.CreateParameter();
            EffYr.ParameterName = "$EffYr";
            command.Parameters.Add(EffYr);

            var PcntComplete = command.CreateParameter();
            PcntComplete.ParameterName = "$PcntComplete";
            command.Parameters.Add(PcntComplete);

            var HeatingSystem = command.CreateParameter();
            HeatingSystem.ParameterName = "$HeatingSystem";
            command.Parameters.Add(HeatingSystem);

            var Sprinklers = command.CreateParameter();
            Sprinklers.ParameterName = "$Sprinklers";
            command.Parameters.Add(Sprinklers);

            var Elevators = command.CreateParameter();
            Elevators.ParameterName = "$Elevators";
            command.Parameters.Add(Elevators);

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
                BldgNbr.Value = record.BldgNbr;
                NbrBldgs.Value = record.NbrBldgs;
                Address.Value = string.IsNullOrWhiteSpace(record?.Address) ? DBNull.Value : record.Address;
                BuildingNumber.Value = string.IsNullOrWhiteSpace(record?.BuildingNumber) ? DBNull.Value : record.BuildingNumber;
                Fraction.Value = string.IsNullOrWhiteSpace(record?.Fraction) ? DBNull.Value : record.Fraction;
                DirectionPrefix.Value = string.IsNullOrWhiteSpace(record?.DirectionPrefix) ? DBNull.Value : record.DirectionPrefix;
                StreetName.Value = string.IsNullOrWhiteSpace(record?.StreetName) ? DBNull.Value : record.StreetName;
                StreetType.Value = string.IsNullOrWhiteSpace(record?.StreetType) ? DBNull.Value : record.StreetType;
                DirectionSuffix.Value = string.IsNullOrWhiteSpace(record?.DirectionSuffix) ? DBNull.Value : record.DirectionSuffix;
                ZipCode.Value = string.IsNullOrWhiteSpace(record?.ZipCode) ? DBNull.Value : record.ZipCode;
                NbrStories.Value = record.NbrStories;
                PredominantUse.Value = string.IsNullOrWhiteSpace(record?.PredominantUse) ? DBNull.Value : record.PredominantUse;
                Shape.Value = string.IsNullOrWhiteSpace(record?.Shape) ? DBNull.Value : record.Shape;
                ConstrClass.Value = string.IsNullOrWhiteSpace(record?.ConstrClass) ? DBNull.Value : record.ConstrClass;
                BldgQuality.Value = string.IsNullOrWhiteSpace(record?.BldgQuality) ? DBNull.Value : record.BldgQuality;
                BldgDescr.Value = string.IsNullOrWhiteSpace(record?.BldgDescr) ? DBNull.Value : record.BldgDescr;
                BldgGrossSqFt.Value = record.BldgGrossSqFt;
                BldgNetSqFt.Value = record.BldgNetSqFt;
                YrBuilt.Value = record.YrBuilt;
                EffYr.Value = record.EffYr;
                PcntComplete.Value = record.PcntComplete;
                HeatingSystem.Value = string.IsNullOrWhiteSpace(record?.HeatingSystem) ? DBNull.Value : record.HeatingSystem;
                Sprinklers.Value = string.IsNullOrWhiteSpace(record?.Sprinklers) ? DBNull.Value : record.Sprinklers;
                Elevators.Value = string.IsNullOrWhiteSpace(record?.Elevators) ? DBNull.Value : record.Elevators;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            PredominantUse = GetPredominantUse();
            Shape = GetShape();
            HeatingSystem = GetHeatingSystem();
            ConstrClass = GetContructionClass();
            BldgQuality = GetBuildingQuality();
            Sprinklers = GetSprinklers();
            Elevators = GetElevators();
            return true;
        }

        public string GetSprinklers()
        {
            return Sprinklers switch
            {
                "Y" => "Yes",
                _ => "No"
            };
        }

        public string GetElevators()
        {
            return Elevators switch
            {
                "Y" => "Yes",
                _ => "No"
            };
        }

        public string GetHeatingSystem()
        {
            return HeatingSystem switch
            {
                "0" => null,
                "1" => "ELECTRIC",
                "10" => "WALL FURNACE",
                "11" => "PACKAGE UNIT",
                "12" => "WARMED AND COOLED AIR",
                "13" => "HOT & CHILLED WATER",
                "14" => "HEAT PUMP",
                "15" => "FLOOR FURNACE",
                "16" => "THRU-WALL HEAT PUMP",
                "17" => "COMPLETE HVAC",
                "18" => "EVAPORATIVE COOLING",
                "19" => "REFRIGERATED COOLING",
                "2" => "ELECTRIC WALL",
                "20" => "NO HEAT",
                "26" => "CONTROL ATMOS., COND. AIR",
                "27" => "CONTROL ATMOS., WARM/COOLED",
                "3" => "FORCED AIR UNIT",
                "4" => "HOT WATER",
                "5" => "HOT WATER-RADIANT",
                "6" => "SPACE HEATERS",
                "7" => "STEAM",
                "8" => "STEAM WITHOUT BOILER",
                "9" => "VENTILATION",
                _ => string.Empty,
            };
        }

        public string GetContructionClass()
        {
            return ConstrClass switch
            {
                "0" => null,
                "1" => "STRUCTURAL STEEL",
                "2" => "REINFORCED CONCRETE",
                "3" => "MASONRY",
                "4" => "WOOD FRAME",
                "5" => "PREFAB STEEL",
                _ => string.Empty,
            };
        }

        public string GetBuildingQuality()
        {
            return BldgQuality switch
            {
                "0" => null,
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

        public string GetShape()
        {
            return Shape switch
            {
                "0" => null,
                "1" => "Approx Square",
                "2" => "Rect or Slight Irreg",
                "3" => "Long Rect or Irreg",
                "4" => "Very Irreg",
                _ => string.Empty,
            };
        }

        public string GetPredominantUse()
        {
            return PredominantUse switch
            {
                "0" => null,
                "102" => "Barn, General Purpose (102)",
                "103" => "Barn, Special Purpose (103)",
                "106" => "Controlled Atmosphere Storage (106)",
                "113" => "Loafing Shed (113)",
                "114" => "Milkhouse Shed (114)",
                "132" => "Individual Livestock Shelter (132)",
                "133" => "Prefabricated Storage Shed (133)",
                "135" => "Greenhouse, Hoop, Arch-Rib, Small (135)",
                "141" => "Greenhouse, Hoop, Arch-Rib, Medium (141)",
                "152" => "Residential Garage - Detached (152)",
                "156" => "Alternative School (156)",
                "157" => "Maintenance Storage Building (157)",
                "162" => "Outbuildings (162)",
                "163" => "Site Improvements (163)",
                "173" => "Church Educational Wing (173)",
                "175" => "Skating Rink, Ice (175)",
                "176" => "Skating Rink, Roller (176)",
                "185" => "Truck Wash (185)",
                "186" => "Light Commercial Manufacturing Utility Bldg (186)",
                "300" => "APARTMENT (300)",
                "301" => "ARMORY (301)",
                "302" => "AUDITORIUM (302)",
                "303" => "AUTOMOBILE SHOWROOM (303)",
                "304" => "BANK (304)",
                "305" => "BARN (305)",
                "306" => "BOWLING ALLEY (306)",
                "308" => "CHURCH WITH SUNDAY SCHOOL (308)",
                "309" => "CHURCH (309)",
                "310" => "CITY CLUB (310)",
                "311" => "CLUBHOUSE (311)",
                "313" => "CONVALESCENT HOSPITAL (313)",
                "314" => "COUNTRY CLUB (314)",
                "315" => "CREAMERY (315)",
                "316" => "DAIRY (316)",
                "317" => "DAIRY SALES BUILDING (317)",
                "318" => "DEPARTMENT STORE (318)",
                "319" => "DISCOUNT STORE (319)",
                "320" => "DISPENSARY (320)",
                "321" => "DORMITORY (321)",
                "322" => "FIRE STATION (STAFFED) (322)",
                "323" => "FRATERNAL BUILDING (323)",
                "324" => "FRATERNITY HOUSE (324)",
                "326" => "GARAGE, STORAGE (326)",
                "327" => "GOVERNMENT BUILDING (327)",
                "328" => "HANGAR, STORAGE (328)",
                "329" => "HANGAR, MAINTENANCE & OFFICE (329)",
                "330" => "HOME FOR THE ELDERLY (330)",
                "331" => "HOSPITAL (331)",
                "332" => "HOTEL, LIMITED (332)",
                "335" => "JAIL-CORRECTIONAL FACILITY (335)",
                "336" => "LAUNDROMAT (336)",
                "337" => "LIBRARY, PUBLIC (337)",
                "338" => "LOFT (338)",
                "339" => "LUMBER STORAGE SHED, HORIZONTAL (339)",
                "340" => "MARKET (340)",
                "341" => "MEDICAL OFFICE (341)",
                "342" => "MORTUARY (342)",
                "343" => "MOTEL, LIMITED (343)",
                "344" => "OFFICE BUILDING (344)",
                "345" => "PARKING STRUCTURE (345)",
                "348" => "Residence (348)",
                "349" => "FAST FOOD RESTAURANT (349)",
                "350" => "RESTAURANT, TABLE SERVICE (350)",
                "351" => "Single-Family Residence (351)",
                "352" => "MULTIPLE RESIDENCE (LOW RISE) (352)",
                "353" => "RETAIL STORE (353)",
                "355" => "Fine Arts & Crafts Building (355)",
                "356" => "Classroom (356)",
                "358" => "Gymnasium (School) (358)",
                "365" => "ELEMENTARY SCHOOL (ENTIRE) (365)",
                "366" => "JUNIOR HIGH SCHOOL (ENTIRE) (366)",
                "368" => "Classroom (College) (368)",
                "369" => "Commons (College) (369)",
                "374" => "Multi-Purp Bldg (College) (374)",
                "377" => "COLLEGE (ENTIRE) (377)",
                "378" => "STABLE (378)",
                "379" => "THEATER, LIVE STAGE (379)",
                "380" => "THEATER, CINEMA (380)",
                "381" => "VETERINARY HOSPITAL (381)",
                "384" => "BARBER SHOP (384)",
                "386" => "MINI-WAREHOUSE (386)",
                "387" => "TRANSIT WAREHOUSE (387)",
                "388" => "UNDERGROUND PARKING STRUCTURE (388)",
                "390" => "Lumber Storage Bldg., Vert. (390)",
                "391" => "MATERIAL STORAGE BUILDING (391)",
                "392" => "INDUSTRIAL ENGINEERING BUILDING (392)",
                "405" => "SKATING RINK (405)",
                "406" => "STORAGE WAREHOUSE (406)",
                "407" => "WAREHOUSE, DISTRIBUTION (407)",
                "408" => "Service Station (408)",
                "409" => "T-HANGAR (409)",
                "410" => "AUTOMOTIVE CENTER (410)",
                "412" => "NEIGHBORHOOD SHOPPING CENTER (412)",
                "413" => "COMMUNITY SHOPPING CENTER (413)",
                "414" => "REGIONAL SHOPPING CENTER (414)",
                "416" => "TENNIS CLUB, INDOOR (416)",
                "417" => "HANDBALL-RACQUETBALL CLUB (417)",
                "418" => "HEALTH CLUB (418)",
                "419" => "CONVENIENCE MARKET (419)",
                "423" => "MINI-LUBE GARAGE (423)",
                "424" => "GROUP CARE HOME (424)",
                "426" => "DAY CARE CENTER (426)",
                "427" => "FIRE STATION (VOLUNTEER) (427)",
                "428" => "HORSE ARENA (428)",
                "431" => "OUTPATIENT SURGICAL CENTER (431)",
                "432" => "RESTROOM BUILDING (432)",
                "434" => "Car Wash - Self Serve (434)",
                "435" => "Car Wash - Drive Thru (435)",
                "436" => "Car Wash - Automatic (436)",
                "441" => "COCKTAIL LOUNGE (441)",
                "442" => "BAR/TAVERN (442)",
                "444" => "DENTAL OFFICE/CLINIC (444)",
                "446" => "SUPERMARKET (446)",
                "447" => "COLD STORAGE FACILITIES (447)",
                "451" => "MULTIPLE RESIDENCE (SENIOR CITIZEN) (451)",
                "453" => "INDUSTRIAL FLEX BUILDINGS (453)",
                "454" => "Shell, Industrial (454)",
                "455" => "AUTO DEALERSHIP, COMPLETE (455)",
                "456" => "Tool Shed (456)",
                "458" => "WAREHOUSE DISCOUNT STORE (458)",
                "459" => "MIXED RETAIL W/RES. UNITS (459)",
                "460" => "Shell, Neigh. Shop. Ctr. (460)",
                "461" => "Shell, Community Shop. Ctr. (461)",
                "462" => "Shell, Regional Shop. Ctr. (462)",
                "465" => "Food Booth - Prefabricated (465)",
                "466" => "Boat Storage Shed (466)",
                "467" => "Boat Storage Building (467)",
                "468" => "SHED, MATERIAL STORAGE (468)",
                "470" => "EQUIPMENT (SHOP) BUILDING (470)",
                "471" => "LIGHT COMMERCIAL UTILITY BUILDING (471)",
                "472" => "EQUIPMENT SHED (472)",
                "473" => "Material Shelter (473)",
                "475" => "POULTRY HOUSE-FLOOR OPERATION (475)",
                "477" => "FARM UTILITY BUILDING (477)",
                "479" => "Farm Utility Storage Shed (479)",
                "481" => "MUSEUM (481)",
                "482" => "CONVENTION CENTER (482)",
                "483" => "FITNESS CENTER (483)",
                "484" => "HIGH SCHOOL (ENTIRE) (484)",
                "485" => "NATATORIUM (485)",
                "486" => "FIELD HOUSES (486)",
                "487" => "VOCATIONAL SCHOOLS (487)",
                "489" => "JAIL - POLICE STATION (489)",
                "490" => "KENNELS (490)",
                "491" => "GOVERNMENT COMMUNITY SERVICE BUILDING (491)",
                "492" => "Shell, Office (492)",
                "494" => "INDUSTRIAL LIGHT MANUFACTURING (494)",
                "495" => "INDUSTRIAL HEAVY MANUFACTURING (495)",
                "496" => "LABORATORIES (496)",
                "497" => "COMPUTER CENTER (497)",
                "498" => "BROADCAST FACILITIES (498)",
                "499" => "Dry Cleaners-Laundry (499)",
                "508" => "Car Wash - Canopy (508)",
                "511" => "Drug Store (511)",
                "513" => "Regional Discount Shopping Center (513)",
                "514" => "Community Center (514)",
                "515" => "Casino (515)",
                "523" => "Golf Cart Storage Building (523)",
                "525" => "MINI WAREHOUSE, HI-RISE (525)",
                "526" => "Service Garage Shed (526)",
                "527" => "MUNICIPAL SERVICE GARAGE (527)",
                "528" => "GARAGE, SERVICE REPAIR (528)",
                "529" => "SNACK BAR (529)",
                "530" => "CAFETERIA (530)",
                "531" => "MINI-MART CONVENIENCE STORE (531)",
                "532" => "FLORIST SHOP (532)",
                "533" => "WAREHOUSE FOOD STORE (533)",
                "534" => "WAREHOUSE SHOWROOM STORE (534)",
                "537" => "Lodge (537)",
                "551" => "ROOMING HOUSE (551)",
                "571" => "Passenger Terminal (571)",
                "573" => "ARCADE (573)",
                "574" => "VISITOR CENTER (574)",
                "577" => "PARKING LEVELS (INTERMEDIATE/UNDER BUILDING) (577)",
                "578" => "Mini-Bank (578)",
                "581" => "POST OFFICE - MAIN(581)",
                "582" => "POST OFFICE - BRANCH(582)",
                "583" => "POST OFFICE - MAIL PROCESSING(583)",
                "584" => "Mega Warehouse (584)",
                "587" => "Shell, Multiple Residence (587)",
                "588" => "MOTEL, EXTENDED STAY (588)",
                "589" => "MULTIPLE RESIDENCES ASSISTED LIVING (LOW RISE)",
                "594" => "HOTEL, FULL SERVICE (594)",
                "595" => "HOTEL, LIMITED SERVICE (595)",
                "596" => "Shell, Apartment (596)",
                "597" => "Mixed Retail w/ Office Units (597)",
                "600" => "Administrative Office (600)",
                "700" => "Mall Anchor Department Store (700)",
                "701" => "BASEMENT, FINISHED (701)",
                "702" => "BASEMENT, SEMIFINISHED (702)",
                "703" => "BASEMENT, UNFINISHED (703)",
                "704" => "BASEMENT, DISPLAY (704)",
                "705" => "BASEMENT, OFFICE (705)",
                "706" => "BASEMENT, PARKING (706)",
                "707" => "BASEMENT, RESIDENT LIVING (707)",
                "708" => "BASEMENT, STORAGE (708)",
                "709" => "BASEMENT, RETAIL (709)",
                "710" => "MULTIPLE RESIDENCE, RETIREMENT COMMUNITY COMPLEX",
                "718" => "Banquet Hall (718)",
                "731" => "HOTEL, FULL SERVICE ECONOMY (731)",
                "732" => "HOTEL, FULL SERVICE MIDSCALE (732)",
                "733" => "HOTEL, FULL SERVICE UPSCALE (733)",
                "734" => "HOTEL, FULL SERVICE UPPER UPSCALE (734)",
                "735" => "HOTEL, FULL SERVICE LUXURY (735)",
                "741" => "HOTEL, LIMITED SERVICE ECONOMY (741)",
                "742" => "HOTEL, LIMITED SERVICE MIDSCALE (742)",
                "743" => "HOTEL, LIMITED SERVICE UPSCALE (743)",
                "744" => "HOTEL, LIMITED SERVICE BUDGET EXTENDED STAY (744)",
                "782" => "Shell, Elderly Assist. Multi. Res. (782)",
                "783" => "Shell, Retirement Community Complex (783)",
                "784" => "Shell, Multiple Res. (Sen. Citizen) (784)",
                "810" => "WAREHOUSE OFFICE (810)",
                "820" => "OPEN OFFICE (820)",
                "830" => "MIXED USE RETAIL (830)",
                "840" => "MIXED USE OFFICE (840)",
                "841" => "HOTEL, FULL SERVICE (841)",
                "842" => "HOTEL, SUITE (842)",
                "843" => "MOTEL, FULL SERVICE (843)",
                "844" => "MOTEL, SUITE (844)",
                "845" => "CONDO, OFFICE (845)",
                "846" => "CONDO, RETAIL (846)",
                "847" => "MIXED USE-OFFICE CONDO (847)",
                "848" => "MIXED USE-RETAIL CONDO (848)",
                "849" => "CONDO, STORAGE (849)",
                "850" => "CONDO, PARKING STRUCTURE (850)",
                "851" => "UNDERGROUND CONDO PARKING (851)",
                "852" => "CONDO HOTEL, FULL SERVICE (852)",
                "853" => "CONDO HOTEL, LIMITED SERVICE (853)",
                "860" => "LINE RETAIL (860)",
                "982" => "CONGREGATE HOUSING (982)",
                "984" => "Luxury Apartment (984)",
                "985" => "Senior Center (985)",
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

    public class CommericalBuildingFeature
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
        public string FeatureType { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public string GrossSqFt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public string NetSqFt { get; set; }
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

            var records = csv.GetRecordsAsync<CommericalBuildingFeature>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into CommericalBuildingFeatures (Id, Major, Minor, ParcelNumber, BldgNbr, FeatureType, GrossSqFt, NetSqFt, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $BldgNbr, $FeatureType, $GrossSqFt, $NetSqFt, $IngestedOn);";

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

            var FeatureType = command.CreateParameter();
            FeatureType.ParameterName = "$FeatureType";
            command.Parameters.Add(FeatureType);

            var GrossSqFt = command.CreateParameter();
            GrossSqFt.ParameterName = "$GrossSqFt";
            command.Parameters.Add(GrossSqFt);

            var NetSqFt = command.CreateParameter();
            NetSqFt.ParameterName = "$NetSqFt";
            command.Parameters.Add(NetSqFt);

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
                FeatureType.Value = record.FeatureType;
                GrossSqFt.Value = record.GrossSqFt;
                NetSqFt.Value = record.NetSqFt;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            FeatureType = GetFeatureType();
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

        public string GetFeatureType()
        {
            return FeatureType switch
            {
                "0" => null,
                "751" => "BALCONY (751)",
                "760" => "MEZZANINES-DISPLAY (760)",
                "761" => "MEZZANINES-OFFICE (761)",
                "763" => "MEZZANINES-STORAGE (763)",
                _ => string.Empty,
            };
        }
    }

    public class CommericalBuildingSection
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
        public string SectionNbr { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public string SectionUse { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public string NbrStories { get; set; }
        [CsvHelper.Configuration.Attributes.Index(6)]
        public string StoryHeight { get; set; }
        [CsvHelper.Configuration.Attributes.Index(7)]
        public string GrossSqFt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(8)]
        public string NetSqFt { get; set; }
        [CsvHelper.Configuration.Attributes.Index(9)]
        public string SectionDescr { get; set; }
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

            var records = csv.GetRecordsAsync<CommericalBuildingSection>();

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into CommericalBuildingSections (Id, Major, Minor, ParcelNumber, BldgNbr, SectionNbr, SectionUse, NbrStories, StoryHeight, GrossSqFt, NetSqFt, SectionDescr, IngestedOn) " +
                $"values ($Id, $Major, $Minor, $ParcelNumber, $BldgNbr, $SectionNbr, $SectionUse, $NbrStories, $StoryHeight, $GrossSqFt, $NetSqFt, $SectionDescr, $IngestedOn);";

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

            var SectionNbr = command.CreateParameter();
            SectionNbr.ParameterName = "$SectionNbr";
            command.Parameters.Add(SectionNbr);

            var SectionUse = command.CreateParameter();
            SectionUse.ParameterName = "$SectionUse";
            command.Parameters.Add(SectionUse);

            var NbrStories = command.CreateParameter();
            NbrStories.ParameterName = "$NbrStories";
            command.Parameters.Add(NbrStories);

            var StoryHeight = command.CreateParameter();
            StoryHeight.ParameterName = "$StoryHeight";
            command.Parameters.Add(StoryHeight);

            var GrossSqFt = command.CreateParameter();
            GrossSqFt.ParameterName = "$GrossSqFt";
            command.Parameters.Add(GrossSqFt);

            var NetSqFt = command.CreateParameter();
            NetSqFt.ParameterName = "$NetSqFt";
            command.Parameters.Add(NetSqFt);

            var SectionDescr = command.CreateParameter();
            SectionDescr.ParameterName = "$SectionDescr";
            command.Parameters.Add(SectionDescr);

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
                SectionNbr.Value = record.SectionNbr;
                SectionUse.Value = record.SectionUse;
                NbrStories.Value = record.NbrStories;
                StoryHeight.Value = record.StoryHeight;
                GrossSqFt.Value = record.GrossSqFt;
                NetSqFt.Value = record.NetSqFt;
                SectionDescr.Value = record.SectionDescr;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            SectionUse = GetSectionUse();
            return true;
        }

        public string GetSectionUse()
        {
            return SectionUse switch
            {
                "0" => null,
                "102" => "Barn, General Purpose (102)",
                "103" => "Barn, Special Purpose (103)",
                "106" => "Controlled Atmosphere Storage (106)",
                "113" => "Loafing Shed (113)",
                "114" => "Milkhouse Shed (114)",
                "132" => "Individual Livestock Shelter (132)",
                "133" => "Prefabricated Storage Shed (133)",
                "135" => "Greenhouse, Hoop, Arch-Rib, Small (135)",
                "141" => "Greenhouse, Hoop, Arch-Rib, Medium (141)",
                "152" => "Residential Garage - Detached (152)",
                "156" => "Alternative School (156)",
                "157" => "Maintenance Storage Building (157)",
                "162" => "Outbuildings (162)",
                "163" => "Site Improvements (163)",
                "173" => "Church Educational Wing (173)",
                "175" => "Skating Rink, Ice (175)",
                "176" => "Skating Rink, Roller (176)",
                "185" => "Truck Wash (185)",
                "186" => "Light Commercial Manufacturing Utility Bldg (186)",
                "300" => "APARTMENT (300)",
                "301" => "ARMORY (301)",
                "302" => "AUDITORIUM (302)",
                "303" => "AUTOMOBILE SHOWROOM (303)",
                "304" => "BANK (304)",
                "305" => "BARN (305)",
                "306" => "BOWLING ALLEY (306)",
                "308" => "CHURCH WITH SUNDAY SCHOOL (308)",
                "309" => "CHURCH (309)",
                "310" => "CITY CLUB (310)",
                "311" => "CLUBHOUSE (311)",
                "313" => "CONVALESCENT HOSPITAL (313)",
                "314" => "COUNTRY CLUB (314)",
                "315" => "CREAMERY (315)",
                "316" => "DAIRY (316)",
                "317" => "DAIRY SALES BUILDING (317)",
                "318" => "DEPARTMENT STORE (318)",
                "319" => "DISCOUNT STORE (319)",
                "320" => "DISPENSARY (320)",
                "321" => "DORMITORY (321)",
                "322" => "FIRE STATION (STAFFED) (322)",
                "323" => "FRATERNAL BUILDING (323)",
                "324" => "FRATERNITY HOUSE (324)",
                "326" => "GARAGE, STORAGE (326)",
                "327" => "GOVERNMENT BUILDING (327)",
                "328" => "HANGAR, STORAGE (328)",
                "329" => "HANGAR, MAINTENANCE & OFFICE (329)",
                "330" => "HOME FOR THE ELDERLY (330)",
                "331" => "HOSPITAL (331)",
                "332" => "HOTEL, LIMITED (332)",
                "335" => "JAIL-CORRECTIONAL FACILITY (335)",
                "336" => "LAUNDROMAT (336)",
                "337" => "LIBRARY, PUBLIC (337)",
                "338" => "LOFT (338)",
                "339" => "LUMBER STORAGE SHED, HORIZONTAL (339)",
                "340" => "MARKET (340)",
                "341" => "MEDICAL OFFICE (341)",
                "342" => "MORTUARY (342)",
                "343" => "MOTEL, LIMITED (343)",
                "344" => "OFFICE BUILDING (344)",
                "345" => "PARKING STRUCTURE (345)",
                "348" => "Residence (348)",
                "349" => "FAST FOOD RESTAURANT (349)",
                "350" => "RESTAURANT, TABLE SERVICE (350)",
                "351" => "Single-Family Residence (351)",
                "352" => "MULTIPLE RESIDENCE (LOW RISE) (352)",
                "353" => "RETAIL STORE (353)",
                "355" => "Fine Arts & Crafts Building (355)",
                "356" => "Classroom (356)",
                "358" => "Gymnasium (School) (358)",
                "365" => "ELEMENTARY SCHOOL (ENTIRE) (365)",
                "366" => "JUNIOR HIGH SCHOOL (ENTIRE) (366)",
                "368" => "Classroom (College) (368)",
                "369" => "Commons (College) (369)",
                "374" => "Multi-Purp Bldg (College) (374)",
                "377" => "COLLEGE (ENTIRE) (377)",
                "378" => "STABLE (378)",
                "379" => "THEATER, LIVE STAGE (379)",
                "380" => "THEATER, CINEMA (380)",
                "381" => "VETERINARY HOSPITAL (381)",
                "384" => "BARBER SHOP (384)",
                "386" => "MINI-WAREHOUSE (386)",
                "387" => "TRANSIT WAREHOUSE (387)",
                "388" => "UNDERGROUND PARKING STRUCTURE (388)",
                "390" => "Lumber Storage Bldg., Vert. (390)",
                "391" => "MATERIAL STORAGE BUILDING (391)",
                "392" => "INDUSTRIAL ENGINEERING BUILDING (392)",
                "405" => "SKATING RINK (405)",
                "406" => "STORAGE WAREHOUSE (406)",
                "407" => "WAREHOUSE, DISTRIBUTION (407)",
                "408" => "Service Station (408)",
                "409" => "T-HANGAR (409)",
                "410" => "AUTOMOTIVE CENTER (410)",
                "412" => "NEIGHBORHOOD SHOPPING CENTER (412)",
                "413" => "COMMUNITY SHOPPING CENTER (413)",
                "414" => "REGIONAL SHOPPING CENTER (414)",
                "416" => "TENNIS CLUB, INDOOR (416)",
                "417" => "HANDBALL-RACQUETBALL CLUB (417)",
                "418" => "HEALTH CLUB (418)",
                "419" => "CONVENIENCE MARKET (419)",
                "423" => "MINI-LUBE GARAGE (423)",
                "424" => "GROUP CARE HOME (424)",
                "426" => "DAY CARE CENTER (426)",
                "427" => "FIRE STATION (VOLUNTEER) (427)",
                "428" => "HORSE ARENA (428)",
                "431" => "OUTPATIENT SURGICAL CENTER (431)",
                "432" => "RESTROOM BUILDING (432)",
                "434" => "Car Wash - Self Serve (434)",
                "435" => "Car Wash - Drive Thru (435)",
                "436" => "Car Wash - Automatic (436)",
                "441" => "COCKTAIL LOUNGE (441)",
                "442" => "BAR/TAVERN (442)",
                "444" => "DENTAL OFFICE/CLINIC (444)",
                "446" => "SUPERMARKET (446)",
                "447" => "COLD STORAGE FACILITIES (447)",
                "451" => "MULTIPLE RESIDENCE (SENIOR CITIZEN) (451)",
                "453" => "INDUSTRIAL FLEX BUILDINGS (453)",
                "454" => "Shell, Industrial (454)",
                "455" => "AUTO DEALERSHIP, COMPLETE (455)",
                "456" => "Tool Shed (456)",
                "458" => "WAREHOUSE DISCOUNT STORE (458)",
                "459" => "MIXED RETAIL W/RES. UNITS (459)",
                "460" => "Shell, Neigh. Shop. Ctr. (460)",
                "461" => "Shell, Community Shop. Ctr. (461)",
                "462" => "Shell, Regional Shop. Ctr. (462)",
                "465" => "Food Booth - Prefabricated (465)",
                "466" => "Boat Storage Shed (466)",
                "467" => "Boat Storage Building (467)",
                "468" => "SHED, MATERIAL STORAGE (468)",
                "470" => "EQUIPMENT (SHOP) BUILDING (470)",
                "471" => "LIGHT COMMERCIAL UTILITY BUILDING (471)",
                "472" => "EQUIPMENT SHED (472)",
                "473" => "Material Shelter (473)",
                "475" => "POULTRY HOUSE-FLOOR OPERATION (475)",
                "477" => "FARM UTILITY BUILDING (477)",
                "479" => "Farm Utility Storage Shed (479)",
                "481" => "MUSEUM (481)",
                "482" => "CONVENTION CENTER (482)",
                "483" => "FITNESS CENTER (483)",
                "484" => "HIGH SCHOOL (ENTIRE) (484)",
                "485" => "NATATORIUM (485)",
                "486" => "FIELD HOUSES (486)",
                "487" => "VOCATIONAL SCHOOLS (487)",
                "489" => "JAIL - POLICE STATION (489)",
                "490" => "KENNELS (490)",
                "491" => "GOVERNMENT COMMUNITY SERVICE BUILDING (491)",
                "492" => "Shell, Office (492)",
                "494" => "INDUSTRIAL LIGHT MANUFACTURING (494)",
                "495" => "INDUSTRIAL HEAVY MANUFACTURING (495)",
                "496" => "LABORATORIES (496)",
                "497" => "COMPUTER CENTER (497)",
                "498" => "BROADCAST FACILITIES (498)",
                "499" => "Dry Cleaners-Laundry (499)",
                "508" => "Car Wash - Canopy (508)",
                "511" => "Drug Store (511)",
                "513" => "Regional Discount Shopping Center (513)",
                "514" => "Community Center (514)",
                "515" => "Casino (515)",
                "523" => "Golf Cart Storage Building (523)",
                "525" => "MINI WAREHOUSE, HI-RISE (525)",
                "526" => "Service Garage Shed (526)",
                "527" => "MUNICIPAL SERVICE GARAGE (527)",
                "528" => "GARAGE, SERVICE REPAIR (528)",
                "529" => "SNACK BAR (529)",
                "530" => "CAFETERIA (530)",
                "531" => "MINI-MART CONVENIENCE STORE (531)",
                "532" => "FLORIST SHOP (532)",
                "533" => "WAREHOUSE FOOD STORE (533)",
                "534" => "WAREHOUSE SHOWROOM STORE (534)",
                "537" => "Lodge (537)",
                "551" => "ROOMING HOUSE (551)",
                "571" => "Passenger Terminal (571)",
                "573" => "ARCADE (573)",
                "574" => "VISITOR CENTER (574)",
                "577" => "PARKING LEVELS (INTERMEDIATE/UNDER BUILDING) (577)",
                "578" => "Mini-Bank (578)",
                "581" => "POST OFFICE - MAIN(581)",
                "582" => "POST OFFICE - BRANCH(582)",
                "583" => "POST OFFICE - MAIL PROCESSING(583)",
                "584" => "Mega Warehouse (584)",
                "587" => "Shell, Multiple Residence (587)",
                "588" => "MOTEL, EXTENDED STAY (588)",
                "589" => "MULTIPLE RESIDENCES ASSISTED LIVING (LOW RISE)",
                "594" => "HOTEL, FULL SERVICE (594)",
                "595" => "HOTEL, LIMITED SERVICE (595)",
                "596" => "Shell, Apartment (596)",
                "597" => "Mixed Retail w/ Office Units (597)",
                "600" => "Administrative Office (600)",
                "700" => "Mall Anchor Department Store (700)",
                "701" => "BASEMENT, FINISHED (701)",
                "702" => "BASEMENT, SEMIFINISHED (702)",
                "703" => "BASEMENT, UNFINISHED (703)",
                "704" => "BASEMENT, DISPLAY (704)",
                "705" => "BASEMENT, OFFICE (705)",
                "706" => "BASEMENT, PARKING (706)",
                "707" => "BASEMENT, RESIDENT LIVING (707)",
                "708" => "BASEMENT, STORAGE (708)",
                "709" => "BASEMENT, RETAIL (709)",
                "710" => "MULTIPLE RESIDENCE, RETIREMENT COMMUNITY COMPLEX",
                "718" => "Banquet Hall (718)",
                "731" => "HOTEL, FULL SERVICE ECONOMY (731)",
                "732" => "HOTEL, FULL SERVICE MIDSCALE (732)",
                "733" => "HOTEL, FULL SERVICE UPSCALE (733)",
                "734" => "HOTEL, FULL SERVICE UPPER UPSCALE (734)",
                "735" => "HOTEL, FULL SERVICE LUXURY (735)",
                "741" => "HOTEL, LIMITED SERVICE ECONOMY (741)",
                "742" => "HOTEL, LIMITED SERVICE MIDSCALE (742)",
                "743" => "HOTEL, LIMITED SERVICE UPSCALE (743)",
                "744" => "HOTEL, LIMITED SERVICE BUDGET EXTENDED STAY (744)",
                "782" => "Shell, Elderly Assist. Multi. Res. (782)",
                "783" => "Shell, Retirement Community Complex (783)",
                "784" => "Shell, Multiple Res. (Sen. Citizen) (784)",
                "810" => "WAREHOUSE OFFICE (810)",
                "820" => "OPEN OFFICE (820)",
                "830" => "MIXED USE RETAIL (830)",
                "840" => "MIXED USE OFFICE (840)",
                "841" => "HOTEL, FULL SERVICE (841)",
                "842" => "HOTEL, SUITE (842)",
                "843" => "MOTEL, FULL SERVICE (843)",
                "844" => "MOTEL, SUITE (844)",
                "845" => "CONDO, OFFICE (845)",
                "846" => "CONDO, RETAIL (846)",
                "847" => "MIXED USE-OFFICE CONDO (847)",
                "848" => "MIXED USE-RETAIL CONDO (848)",
                "849" => "CONDO, STORAGE (849)",
                "850" => "CONDO, PARKING STRUCTURE (850)",
                "851" => "UNDERGROUND CONDO PARKING (851)",
                "852" => "CONDO HOTEL, FULL SERVICE (852)",
                "853" => "CONDO HOTEL, LIMITED SERVICE (853)",
                "860" => "LINE RETAIL (860)",
                "982" => "CONGREGATE HOUSING (982)",
                "984" => "Luxury Apartment (984)",
                "985" => "Senior Center (985)",
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
