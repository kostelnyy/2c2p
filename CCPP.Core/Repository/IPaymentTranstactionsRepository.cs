using CCPP.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCPP.Core.Repository
{
    public interface IPaymentTranstactionsRepository
    {
        Task AddAsync(IEnumerable<PaymentTransaction> paymentTransactions);
        Task SaveChangesAsync();
    }
}
