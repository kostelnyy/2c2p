using CCPP.Core.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CCPP.Data.Queries
{
    public class PaymentTransactionQueries : IPaymentTranstactionQueries
    {
        public Task<IEnumerable<PaymentTransactionDto>> GetTransactions(GetPaymentTransactionsQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
