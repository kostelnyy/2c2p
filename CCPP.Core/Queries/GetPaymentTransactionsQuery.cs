using CCPP.Core.Domain;
using System;

namespace CCPP.Core.Queries
{
    public class GetPaymentTransactionsQuery
    {
        public string Currency { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public PaymentTransactionStatus? Status { get; set; }
    }
}
