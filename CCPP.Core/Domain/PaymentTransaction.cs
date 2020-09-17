using System;

namespace CCPP.Core.Domain
{
    public class PaymentTransaction
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentTransactionStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
