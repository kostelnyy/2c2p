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

            modelBuilder.Entity<PaymentTranstaction>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id)
                    .HasMaxLength(50);
                b.Property(x => x.Currency)
                    .IsRequired()
                    .HasMaxLength(3);
                b.Property(x => x.Status)
                    .HasConversion<string>();
                b.HasIndex(x => x.Currency);
                b.HasIndex(x => x.TransactionDate);
                b.HasIndex(x => x.Status);
            });
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
