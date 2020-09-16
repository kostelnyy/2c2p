using CCPP.Core.Domain;
using CCPP.Core.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCPP.Data.Repository
{
    public class PaymentTranstactionsRepository : IPaymentTranstactionsRepository
    {
        public async Task AddAsync(IEnumerable<PaymentTranstaction> paymentTransactions)
        {
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await Task.CompletedTask;
        }
    }
}
