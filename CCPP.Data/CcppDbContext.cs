using CCPP.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CCPP.Data
{
    public class CcppDbContext : DbContext
    {
        public CcppDbContext(DbContextOptions<CcppDbContext> options) : base(options)
        {
        }

        public DbSet<PaymentTranstaction> PaymentTranstactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("transactions");

            modelBuilder.Entity<PaymentTranstaction>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<PaymentTranstaction>()
                .Property(x => x.Id)
                .HasMaxLength(50);

            modelBuilder.Entity<PaymentTranstaction>()
                .Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(3);

            modelBuilder.Entity<PaymentTranstaction>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<PaymentTranstaction>()
                .HasIndex(x => x.Currency);

            modelBuilder.Entity<PaymentTranstaction>()
                .HasIndex(x => x.TransactionDate);

            modelBuilder.Entity<PaymentTranstaction>()
                .HasIndex(x => x.Status);
        }
    }

    public class CcppDbContextFactory : IDesignTimeDbContextFactory<CcppDbContext>
    {
        public CcppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CcppDbContext>()
                 .UseSqlServer("Server=.;Initial Catalog=CCPP;Integrated Security=true");

            return new CcppDbContext(optionsBuilder.Options);
        }
    }
}
