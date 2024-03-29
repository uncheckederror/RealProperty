﻿using Microsoft.EntityFrameworkCore;

namespace eRealProperty.Models
{
    public class eRealPropertyContext : DbContext
    {
        public DbSet<RealPropertyAccount> RealPropertyAccounts { get; set; }
        public DbSet<RealPropertyAccountTaxYear> RealPropertyAccountTaxYears { get; set; }
        public DbSet<PropertyParcel> PropertyParcels { get; set; }
        public DbSet<LegalDiscription> LegalDiscriptions { get; set; }
        public DbSet<TaxLevy> LevyCodes { get; set; }
        public DbSet<ResidentialBuilding> ResidentialBuildings { get; set; }
        public DbSet<RealPropertyAccountSale> Sales { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewDescription> ReviewDescriptions { get; set; }
        public DbSet<Permit> Permits { get; set; }
        public DbSet<PermitDetailHistory> PermitDetailHistories { get; set; }
        public DbSet<CommericalBuilding> CommericalBuildings { get; set; }
        public DbSet<CommericalBuildingSection> CommericalBuildingSections { get; set; }
        public DbSet<CommericalBuildingFeature> CommericalBuildingFeatures { get; set; }
        public DbSet<CondoComplex> CondoComplexes { get; set; }
        public DbSet<CondoUnit> CondoUnits { get; set; }
        public DbSet<ApartmentComplex> ApartmentComplexes { get; set; }
        public DbSet<UnitBreakdown> UnitBreakdowns { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RealPropertyAccount>()
                .HasIndex(b => b.AcctNbr);

            modelBuilder.Entity<RealPropertyAccountTaxYear>()
                .HasIndex(b => b.ParcelNumber);

            modelBuilder.Entity<PropertyParcel>()
                .HasIndex(b => b.ParcelNumber);

            modelBuilder.Entity<LegalDiscription>()
                .HasIndex(b => b.ParcelNumber);

            modelBuilder.Entity<ResidentialBuilding>()
                .HasIndex(b => b.ParcelNumber);

            modelBuilder.Entity<TaxLevy>()
                .HasIndex(b => b.LevyCode);

            modelBuilder.Entity<RealPropertyAccountSale>()
                .HasIndex(b => b.ParcelNumber);

            modelBuilder.Entity<Review>()
                .HasIndex(x => x.ParcelNumber);

            modelBuilder.Entity<ReviewDescription>()
                .HasIndex(x => x.AppealNbr);

            modelBuilder.Entity<Permit>()
                .HasIndex(x => x.ParcelNumber);

            modelBuilder.Entity<PermitDetailHistory>()
                .HasIndex(x => x.PermitNbr);

            modelBuilder.Entity<CommericalBuilding>()
                .HasIndex(x => x.ParcelNumber);

            modelBuilder.Entity<CommericalBuildingSection>()
                .HasIndex(x => x.ParcelNumber);

            modelBuilder.Entity<CommericalBuildingFeature>()
                .HasIndex(x => x.ParcelNumber);

            modelBuilder.Entity<CondoComplex>()
                .HasIndex(x => x.Major);

            modelBuilder.Entity<CondoUnit>()
                .HasIndex(x => x.ParcelNumber);

            modelBuilder.Entity<ApartmentComplex>()
                .HasIndex(x => x.ParcelNumber);

            modelBuilder.Entity<UnitBreakdown>()
                .HasIndex(x => x.ParcelNumber);
        }

        public eRealPropertyContext(DbContextOptions<eRealPropertyContext> options)
            : base(options)
        {

        }
    }
}
