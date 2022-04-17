using Microsoft.EntityFrameworkCore;

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
        }

        public eRealPropertyContext(DbContextOptions<eRealPropertyContext> options)
            : base(options)
        {

        }
    }
}
