using CsvHelper;
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
    public class RealPropertyAccountSale
    {
        [Key]
        [Ignore]
        public Guid Id { get; set; }
        public int ExciseTaxNbr { get; set; }
        public string Major { get; set; }
        public string Minor { get; set; }
        [Ignore]
        public string ParcelNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public long SalePrice { get; set; }
        public string RecordingNbr { get; set; }
        public string Volume { get; set; }
        public string Page { get; set; }
        public string PlatNbr { get; set; }
        public string PlatType { get; set; }
        public string PlatLot { get; set; }
        public string PlatBlock { get; set; }
        public string SellerName { get; set; }
        public string BuyerName { get; set; }
        public string PropertyType { get; set; }
        public string PrincipalUse { get; set; }
        public string SaleInstrument { get; set; }
        public char AFForestLand { get; set; }
        public char AFCurrentUseLand { get; set; }
        public char AFNonProfitUse { get; set; }
        public char AFHistoricProperty { get; set; }
        public string SaleReason { get; set; }
        public string PropertyClass { get; set; }
        public string SaleWarning { get; set; }
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
                CacheFields = true
            };

            using var transaction = await context.Database.BeginTransactionAsync();
            using var reader = new StreamReader(pathToCSV);
            using var csv = new CsvReader(reader, config);

            var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText =
                $"insert into Sales (Id, ExciseTaxNbr, DocumentDate, SalePrice, RecordingNbr, Volume, Page, PlatNbr, PlatType, PlatLot, Major, Minor, ParcelNumber, PlatBlock, SellerName, BuyerName, PropertyType, PrincipalUse, SaleInstrument, AFForestLand, AFCurrentUseLand, AFNonProfitUse, AFHistoricProperty, SaleReason, PropertyClass, SaleWarning, IngestedOn) " +
                $"values ($Id, $ExciseTaxNbr, $DocumentDate, $SalePrice, $RecordingNbr, $Volume, $Page, $PlatNbr, $PlatType, $PlatLot, $Major, $Minor, $ParcelNumber, $PlatBlock, $SellerName, $BuyerName, $PropertyType, $PrincipalUse, $SaleInstrument, $AFForestLand, $AFCurrentUseLand, $AFNonProfitUse, $AFHistoricProperty, $SaleReason, $PropertyClass, $SaleWarning, $IngestedOn);";

            var Id = command.CreateParameter();
            Id.ParameterName = "$Id";
            command.Parameters.Add(Id);

            var ExciseTaxNbr = command.CreateParameter();
            ExciseTaxNbr.ParameterName = "$ExciseTaxNbr";
            command.Parameters.Add(ExciseTaxNbr);

            var DocumentDate = command.CreateParameter();
            DocumentDate.ParameterName = "$DocumentDate";
            command.Parameters.Add(DocumentDate);

            var SalePrice = command.CreateParameter();
            SalePrice.ParameterName = "$SalePrice";
            command.Parameters.Add(SalePrice);

            var RecordingNbr = command.CreateParameter();
            RecordingNbr.ParameterName = "$RecordingNbr";
            command.Parameters.Add(RecordingNbr);

            var Volume = command.CreateParameter();
            Volume.ParameterName = "$Volume";
            command.Parameters.Add(Volume);

            var Page = command.CreateParameter();
            Page.ParameterName = "$Page";
            command.Parameters.Add(Page);

            var PlatNbr = command.CreateParameter();
            PlatNbr.ParameterName = "$PlatNbr";
            command.Parameters.Add(PlatNbr);

            var PlatType = command.CreateParameter();
            PlatType.ParameterName = "$PlatType";
            command.Parameters.Add(PlatType);
            var PlatLot = command.CreateParameter();
            PlatLot.ParameterName = "$PlatLot";
            command.Parameters.Add(PlatLot);

            var Major = command.CreateParameter();
            Major.ParameterName = "$Major";
            command.Parameters.Add(Major);

            var Minor = command.CreateParameter();
            Minor.ParameterName = "$Minor";
            command.Parameters.Add(Minor);

            var ParcelNumber = command.CreateParameter();
            ParcelNumber.ParameterName = "$ParcelNumber";
            command.Parameters.Add(ParcelNumber);

            var PlatBlock = command.CreateParameter();
            PlatBlock.ParameterName = "$PlatBlock";
            command.Parameters.Add(PlatBlock);

            var SellerName = command.CreateParameter();
            SellerName.ParameterName = "$SellerName";
            command.Parameters.Add(SellerName);

            var BuyerName = command.CreateParameter();
            BuyerName.ParameterName = "$BuyerName";
            command.Parameters.Add(BuyerName);

            var PropertyType = command.CreateParameter();
            PropertyType.ParameterName = "$PropertyType";
            command.Parameters.Add(PropertyType);

            var PrincipalUse = command.CreateParameter();
            PrincipalUse.ParameterName = "$PrincipalUse";
            command.Parameters.Add(PrincipalUse);

            var SaleInstrument = command.CreateParameter();
            SaleInstrument.ParameterName = "$SaleInstrument";
            command.Parameters.Add(SaleInstrument);

            var AFForestLand = command.CreateParameter();
            AFForestLand.ParameterName = "$AFForestLand";
            command.Parameters.Add(AFForestLand);

            var AFCurrentUseLand = command.CreateParameter();
            AFCurrentUseLand.ParameterName = "$AFCurrentUseLand";
            command.Parameters.Add(AFCurrentUseLand);

            var AFNonProfitUse = command.CreateParameter();
            AFNonProfitUse.ParameterName = "$AFNonProfitUse";
            command.Parameters.Add(AFNonProfitUse);

            var AFHistoricProperty = command.CreateParameter();
            AFHistoricProperty.ParameterName = "$AFHistoricProperty";
            command.Parameters.Add(AFHistoricProperty);

            var SaleReason = command.CreateParameter();
            SaleReason.ParameterName = "$SaleReason";
            command.Parameters.Add(SaleReason);

            var PropertyClass = command.CreateParameter();
            PropertyClass.ParameterName = "$PropertyClass";
            command.Parameters.Add(PropertyClass);

            var SaleWarning = command.CreateParameter();
            SaleWarning.ParameterName = "$SaleWarning";
            command.Parameters.Add(SaleWarning);

            var IngestedOn = command.CreateParameter();
            IngestedOn.ParameterName = "$IngestedOn";
            command.Parameters.Add(IngestedOn);

            var records = csv.GetRecordsAsync<RealPropertyAccountSale>();

            await foreach (var record in records)
            {
                record.Id = Guid.NewGuid();
                record.IngestedOn = DateTime.Now;
                record.TranslateFieldsUsingLookupsToText();

                Id.Value = record.Id;
                ExciseTaxNbr.Value = record.ExciseTaxNbr;
                DocumentDate.Value = record.DocumentDate;
                SalePrice.Value = record.SalePrice;
                RecordingNbr.Value = string.IsNullOrWhiteSpace(record?.RecordingNbr) ? DBNull.Value : record.RecordingNbr;
                Volume.Value = string.IsNullOrWhiteSpace(record?.Volume) ? DBNull.Value : record.Volume;
                Page.Value = string.IsNullOrWhiteSpace(record?.Page) ? DBNull.Value : record.Page;
                PlatNbr.Value = string.IsNullOrWhiteSpace(record?.PlatNbr) ? DBNull.Value : record.PlatNbr;
                PlatType.Value = string.IsNullOrWhiteSpace(record?.PlatType) ? DBNull.Value : record.PlatType;
                PlatLot.Value = string.IsNullOrWhiteSpace(record?.PlatLot) ? DBNull.Value : record.PlatType;
                Major.Value = string.IsNullOrWhiteSpace(record?.Major) ? DBNull.Value : record.Major;
                Minor.Value = string.IsNullOrWhiteSpace(record?.Minor) ? DBNull.Value : record.Minor;
                ParcelNumber.Value = string.IsNullOrWhiteSpace(record?.ParcelNumber) ? DBNull.Value : record.ParcelNumber;
                PlatBlock.Value = string.IsNullOrWhiteSpace(record?.PlatBlock) ? DBNull.Value : record.PlatBlock;
                SellerName.Value = string.IsNullOrWhiteSpace(record?.SellerName) ? DBNull.Value : record.SellerName;
                BuyerName.Value = string.IsNullOrWhiteSpace(record?.BuyerName) ? DBNull.Value : record.BuyerName;
                PropertyType.Value = string.IsNullOrWhiteSpace(record?.PropertyType) ? DBNull.Value : record.PropertyType;
                PrincipalUse.Value = string.IsNullOrWhiteSpace(record?.PrincipalUse) ? DBNull.Value : record.PrincipalUse;
                SaleInstrument.Value = string.IsNullOrWhiteSpace(record?.SaleInstrument) ? DBNull.Value : record.SaleInstrument;
                AFForestLand.Value = record.AFForestLand;
                AFCurrentUseLand.Value = record.AFCurrentUseLand;
                AFNonProfitUse.Value = record.AFNonProfitUse;
                AFHistoricProperty.Value = record.AFHistoricProperty;
                SaleReason.Value = string.IsNullOrWhiteSpace(record?.SaleReason) ? DBNull.Value : record.SaleReason;
                PropertyClass.Value = string.IsNullOrWhiteSpace(record?.PropertyClass) ? DBNull.Value : record.PropertyClass;
                SaleWarning.Value = string.IsNullOrWhiteSpace(record?.SaleWarning) ? DBNull.Value : record.SaleWarning;
                IngestedOn.Value = record.IngestedOn;

                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
            return true;
        }

        public static async Task IngestByParcelNumberAsync(string parcelNumber, eRealPropertyContext context)
        {
            var pathToCSV = Path.Combine(AppContext.BaseDirectory, "SourceData\\EXTR_RPSale.csv");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(pathToCSV))
            using (var csv = new CsvReader(reader, config))
            {
                var major = parcelNumber.Substring(0, 6);
                var minor = parcelNumber.Substring(6, 4);

                context.ChangeTracker.AutoDetectChangesEnabled = false;

                csv.Read();
                csv.ReadHeader();
                var records = new List<RealPropertyAccountSale>();

                while (csv.Read())
                {
                    var record = new RealPropertyAccountSale
                    {
                        Id = Guid.NewGuid(),
                        ExciseTaxNbr = csv.GetField<int>("ExciseTaxNbr"),
                        DocumentDate = csv.GetField<DateTime>("DocumentDate"),
                        SalePrice = csv.GetField<long>("SalePrice"),
                        RecordingNbr = csv.GetField<string>("RecordingNbr"),
                        Volume = csv.GetField<string>("Volume"),
                        Page = csv.GetField<string>("Page"),
                        PlatNbr = csv.GetField<string>("PlatNbr"),
                        PlatType = csv.GetField<string>("PlatType"),
                        PlatLot = csv.GetField<string>("PlatLot"),
                        Major = csv.GetField<string>("Major"),
                        Minor = csv.GetField<string>("Minor"),
                        PlatBlock = csv.GetField<string>("PlatBlock"),
                        SellerName = csv.GetField<string>("SellerName"),
                        BuyerName = csv.GetField<string>("BuyerName"),
                        PropertyType = csv.GetField<string>("PropertyType"),
                        PrincipalUse = csv.GetField<string>("PrincipalUse"),
                        SaleInstrument = csv.GetField<string>("SaleInstrument"),
                        AFForestLand = csv.GetField<char>("AFForestLand"),
                        AFCurrentUseLand = csv.GetField<char>("AFCurrentUseLand"),
                        AFNonProfitUse = csv.GetField<char>("AFNonProfitUse"),
                        AFHistoricProperty = csv.GetField<char>("AFHistoricProperty"),
                        SaleReason = csv.GetField<string>("SaleReason"),
                        PropertyClass = csv.GetField<string>("PropertyClass"),
                        SaleWarning = csv.GetField<string>("SaleWarning"),
                        IngestedOn = DateTime.Now
                    };


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
        }

        public bool TranslateFieldsUsingLookupsToText()
        {
            ParcelNumber = GetParcelNumber();
            PropertyType = GetPropertyType();
            PrincipalUse = GetPrincupalUse();
            SaleInstrument = GetSaleInstrument();
            SaleReason = GetSaleReason();
            PropertyClass = GetPropertyClass();
            SaleWarning = GetSaleWarning();

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

        // Lookup 1
        // The type of property as reported on the Excise Tax affidavit.
        public string GetPropertyType()
        {
            if (!string.IsNullOrEmpty(PropertyType))
            {
                return PropertyType switch
                {
                    "1" => "Land only",
                    "2" => "Land with new Building",
                    "3" => "Land with previously used building",
                    "4" => "Land with Mobile Home",
                    "5" => "Timber Only",
                    "6" => "Building Only",
                    "9" => "Land with mobile home",
                    "10" => "Land With New Building",
                    "11" => "Household, Single Family Units",
                    "12" => "Multiple family residence (Residential, 2-4 units)",
                    "13" => "Multiple family residence (Residential, 5+ units) ",
                    "14" => "Residential condominiums",
                    "15" => "Mobile home parks or courts",
                    "16" => "Hotels/motels",
                    "17" => "Institutional lodging",
                    "18" => "All other residential not elsewhere coded",
                    "19" => "Vacation and cabin",
                    "21" => "Food and kindred products",
                    "22" => "Textile mill products",
                    "23" => "Apparel & other finished products",
                    "24" => "Lumber and wood products (except furniture)",
                    "25" => "Furniture and fixtures",
                    "26" => "Paper and allied products",
                    "27" => "Printing and publishing",
                    "28" => "Chemicals",
                    "29" => "Petroleum refining and related industries",
                    "30" => "Rubber and miscellaneous plastic products",
                    "31" => "Leather and leather products",
                    "32" => "Stone, clay and glass products",
                    "33" => "Primary metal industries",
                    "34" => "Fabricated metal products",
                    "35" => "Professional scientific, controlling instruments; optical goods",
                    "39" => "Miscellaneous manufacturing",
                    "41" => "Railroad/transit transportation",
                    "42" => "Motor vehicle transportation",
                    "43" => "Aircraft transportation",
                    "44" => "Marine craft transportation",
                    "45" => "Highway and street right of way",
                    "46" => "Automobile parking",
                    "47" => "Communication",
                    "48" => "Utilities",
                    "49" => "Other trans., comm, & util. not classified",
                    "50" => "Condominiums - other than residential condominiums",
                    "51" => "Wholesale trade",
                    "52" => "Retail trade-bldg materials, hardware, farm equip",
                    "53" => "Retail trade - general merchandise",
                    "54" => "Retail trade - food",
                    "55" => "Retail trade-autom., marine craft, aircraft",
                    "56" => "Retail trade - apparel and accessories",
                    "57" => "Retail trade-furniture, home furnishings, equip.",
                    "58" => "Retail trade-eating & drinking",
                    "59" => "Tenant occupied, commercial properties",
                    "61" => "Finance, insurance, and real estate services",
                    "62" => "Personal services",
                    "63" => "Business services",
                    "64" => "Repair services",
                    "65" => "Professional services (medical, dental, etc.)",
                    "66" => "Contract construction services",
                    "67" => "Governmental services",
                    "68" => "Educational services",
                    "69" => "Miscellaneous services",
                    "71" => "Cultural activities and nature exhibitions",
                    "72" => "Public assembly",
                    "73" => "Amusements",
                    "74" => "Recreational activities (gold courses, etc.)",
                    "75" => "Resorts and group camps ",
                    "76" => "Parks",
                    "79" => "Other cultural, entertainment, and recreational",
                    "80" => "Water or Mineral rights",
                    "81" => "Agriculture (not classified under current use law)",
                    "82" => "Agriculture related activities",
                    "83" => "Agr classified under current use chapter 84.34 RCW",
                    "84" => "Fishing activities and related services",
                    "85" => "Mining activities and related services",
                    "86" => "Standing Timber (separate from land)",
                    "88" => "Forest land designated under chapter 84.33 RCW",
                    "89" => "Other resource production",
                    "91" => "Undeveloped land (land only)",
                    "92" => "Noncommercial forest",
                    "93" => "Water areas",
                    "94" => "Open space land classified under chapter 84.34 RCW",
                    "95" => "Timberland classified under chapter 84.34 RCW",
                    "96" => "Improvements on leased land",
                    "99" => "Other undeveloped land",
                    _ => null
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 2
        // The primary use of the property as reported on the Excise Tax affidavit.
        public string GetPrincupalUse()
        {
            if (!string.IsNullOrEmpty(PrincipalUse))
            {
                return PrincipalUse switch
                {
                    "1" => "Agricultural",
                    "2" => "Condominium",
                    "3" => "Recreational",
                    "4" => "Apartments with 4 or more units",
                    "5" => "Industrial",
                    "6" => "Residential",
                    "7" => "Commerical",
                    "8" => "Mobile Home",
                    "9" => "Timber",
                    "10" => "Other",
                    "11" => "Commerical or Industrial",
                    _ => null
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 6
        // The type of document as reported on the Excise Tax affidavit.
        public string GetSaleInstrument()
        {
            if (!string.IsNullOrEmpty(SaleInstrument))
            {
                return SaleInstrument switch
                {
                    "1" => "None",
                    "2" => "Warranty Deed",
                    "3" => "Statutory Warranty Deed",
                    "4" => "Special Warrenty Deed",
                    "5" => "Corporate Warrenty Deed",
                    "6" => "Assumption Warrenty Deed",
                    "7" => "Grant Deed",
                    "8" => "Contract (equity)",
                    "9" => "Contract (installment)",
                    "10" => "Real Estate Contract",
                    "11" => "Purchaser's Assignment",
                    "13" => "Seller's Assignment",
                    "15" => "Quit Claim Deed",
                    "18" => "Trustees' Deed",
                    "19" => "Executor's Deed",
                    "20" => "Fiduciary Deed",
                    "21" => "Sheriff's Deed",
                    "22" => "Bargin and Sales Deed",
                    "23" => "Receivers Deed",
                    "24" => "Deed of Personal Rep",
                    "25" => "Judgement Per Stipulation",
                    "26" => "Other - See Affidavit",
                    "27" => "Deed",
                    "28" => "Forfeiture Real Estate Contract",
                    _ => null
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 5
        public string GetSaleReason()
        {
            if (!string.IsNullOrEmpty(SaleReason))
            {
                return SaleReason switch
                {
                    "1" => "None",
                    "2" => "Assumption",
                    "3" => "Mortgage Assumption",
                    "4" => "Foreclosure",
                    "5" => "Trust",
                    "6" => "Executor-to admin guardian",
                    "7" => "Testamentary Trust",
                    "8" => "Estate Settlement",
                    "9" => "Settlement",
                    "10" => "Property Settlement",
                    "11" => "Divorce Settlementt",
                    "12" => "Tenancy Partition",
                    "13" => "Community Prop Established",
                    "14" => "Partial Int - love,aff,gft",
                    "15" => "Easement",
                    "16" => "Correction (refiling)",
                    "17" => "Trade",
                    "18" => "Other",
                    "19" => "Quit Claim Deed - gift/full or part interest",
                    _ => null
                };
            }
            else
            {
                return null;
            }
        }

        // Lookup 4
        public string GetPropertyClass()
        {
            if (!string.IsNullOrEmpty(PropertyClass))
            {
                return PropertyClass switch
                {
                    "1" => "C/I-Land only",
                    "2" => "C/I-Imp prop; no condo/MH",
                    "3" => "C/I-Condominium",
                    "4" => "C/I-Air rights only",
                    "5" => "C/I-Imp prop excl air rights",
                    "6" => "C/I-Land or bldg; no split",
                    "7" => "Res-Land only",
                    "8" => "Res-Improved property",
                    "9" => "Res or C/I-Mobile Home",
                    _ => null
                };
            }
            else
            {
                return null;
            }
        }

        public string GetSaleWarning()
        {
            if (!string.IsNullOrEmpty(SaleWarning))
            {
                var warnings = SaleWarning.Split(" ");
                var output = new List<string>();

                foreach (var warning in warnings)
                {
                    var parsed = GetSpecificWarning(warning);
                    if (parsed is not null)
                    {
                        output.Add(parsed);
                    }
                }

                return output.Any() ? string.Join(", ", output) : SaleWarning; ;
            }
            else
            {
                return null;
            }

            static string GetSpecificWarning(string warning)
            {
                return warning switch
                {
                    "1" => "PERSONAL PROPERTY INCLUDED",
                    "2" => "1031 TRADE",
                    "3" => "CONTRACT OR CASH SALE",
                    "4" => "PRESALE",
                    "5" => "FULL SALES PRICE NOT REPORTED",
                    "6" => "REFUND",
                    "7" => "QUESTIONABLE PER SALES IDENTIFICATION",
                    "8" => "QUESTIONABLE PER APPRAISAL",
                    "9" => "QUESTIONABLE PER MAINFRAME SYSTEM (Obsolete code)",
                    "10" => "TEAR DOWN",
                    "11" => "CORPORATE AFFILIATES",
                    "12" => "ESTATE ADMINISTRATOR, GUARDIAN, OR EXECUTOR",
                    "13" => "BANKRUPTCY - RECEIVER OR TRUSTEE",
                    "14" => "SHERIFF / TAX SALE",
                    "15" => "NO MARKET EXPOSURE",
                    "16" => "GOV'T TO GOV'T",
                    "17" => "NON-PROFIT ORGANIZATION",
                    "18" => "QUIT CLAIM DEED",
                    "19" => "SELLER'S OR PURCHASER'S ASSIGNMENT",
                    "20" => "CORRECTION DEED",
                    "21" => "TRADE",
                    "22" => "PARTIAL INTEREST (1/3, 1/2, Etc.)",
                    "23" => "FORCED SALE",
                    "24" => "EASEMENT OR RIGHT-OF-WAY",
                    "25" => "FULFILLMENT OF CONTRACT DEED",
                    "26" => "IMP. CHARACTERISTICS CHANGED SINCE SALE",
                    "27" => "TIMBER AND FOREST LAND",
                    "28" => "NEW PLAT (WITH LESS THAN 20% SOLD)",
                    "29" => "SEGREGATION AND/OR MERGER",
                    "30" => "PERSONAL PROPERTY INCLUDED",
                    "31" => "EXEMPT FROM EXCISE TAX",
                    "32" => "$1,000 SALE OR LESS",
                    "33" => "LEASE OR LEASE-HOLD",
                    "34" => "CHANGE OF USE",
                    "35" => "OPEN SPACE DESIGNATION CONTINUED/OK'D AFTER SALE",
                    "36" => "PLOTTAGE",
                    "37" => "SECURING OF DEBT",
                    "38" => "DIVORCE",
                    "39" => "ASSUMPTION OF MORTGAGE W/NO ADDL CONSIDERATION PD",
                    "40" => "RELOCATION - SALE TO SERVICE",
                    "41" => "RELOCATION - SALE BY SERVICE",
                    "42" => "DEVELOPMENT RIGHTS TO CNTY,CTY,OR PRVT DEVELOPER",
                    "43" => "DEVELOPMENT RIGHTS PARCEL TO PRVT SECTOR",
                    "44" => "TENANT",
                    "45" => "MULTI-PARCEL SALE",
                    "46" => "NON-REPRESENTATIVE SALE",
                    "47" => "NON-CONVENTIONAL HEATING SYSTEM",
                    "48" => "CONDO WITH GARAGE, MOORAGE, OR STORAGE",
                    "49" => "MOBILE HOME",
                    "50" => "CONDO WHOLESALE",
                    "51" => "RELATED PARTY, FRIEND, OR NEIGHBOR",
                    "52" => "STATEMENT TO DOR",
                    "53" => "RESIDUAL SALES",
                    "54" => "AFFORDABLE HOUSING SALES",
                    "55" => "SHELL",
                    "56" => "BUILDER OR DEVELOPER SALES",
                    "57" => "SELLING OR BUYING COSTS AFFECTING SALE PRICE",
                    "58" => "PRELIMINARY SHORTPLAT APPROVAL",
                    "59" => "BULK PORTFOLIO SALE",
                    "60" => "SHORT SALE",
                    "61" => "FINANCIAL INSTITUTION RESALE",
                    "62" => "AUCTION SALE",
                    "63" => "SALE PRICE UPDATED BY SALES ID GROUP",
                    "64" => "SALES/LEASEBACK",
                    "65" => "PLANS AND PERMITS",
                    "66" => "CONDEMNATION/EMINENT DOMAIN",
                    "67" => "GOV'T TO NON-GOV'T",
                    "68" => "NON-GOV'T TO GOV'T",
                    "69" => "NET LEASE SALE",
                    "70" => "BUILDING ONLY",
                    "71" => "PARKING EASEMENT",
                    "72" => "PARKING STALLS",
                    "73" => "COVID IMPACT",
                    _ => null
                };
            }
        }
    }
}
