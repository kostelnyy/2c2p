using System;
using System.Collections.Generic;
using System.Text;

namespace CCPP.Core.Queries
{
    public class GetPaymentTransactionsQuery
    {
        public string Currency { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public PaymentTransactionStatusDto? Status { get; set; }
    }
}
