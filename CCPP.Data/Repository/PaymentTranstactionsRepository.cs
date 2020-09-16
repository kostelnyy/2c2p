using CCPP.Core.Domain;
using CCPP.Core.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCPP.Data.Repository
{
    public class PaymentTranstactionsRepository : IPaymentTranstactionsRepository
    {
        private readonly CcppDbContext _dbContext;

        public PaymentTranstactionsRepository(CcppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(IEnumerable<PaymentTranstaction> paymentTransactions)
        {
            await _dbContext.AddRangeAsync(paymentTransactions);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
