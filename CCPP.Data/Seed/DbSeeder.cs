using CCPP.Core.Domain;
using System;
using System.Linq;

namespace CCPP.Data.Seed
{
    public class DbSeeder
    {
        public static void Seed(CcppDbContext context)
        {
            if (!context.PaymentTranstactions.Any())
            {
                context.PaymentTranstactions.Add(new PaymentTranstaction
                {
                    Id = "PaymentTest001",
                    Amount = 100m,
                    Currency = "EUR",
                    Status = PaymentTranstactionStatus.Approved,
                    TransactionDate = DateTime.UtcNow
                });
                context.SaveChanges();
            }
        }
    }
}
