using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCPP.Core.Queries
{
    public interface IPaymentTranstactionQueries
    {
        Task<IEnumerable<PaymentTransactionDto>> GetTransactions(GetPaymentTransactionsQuery query);
    }
}
