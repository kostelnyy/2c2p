using System;

namespace CCPP.Core.Domain
{
    public class PaymentTranstaction
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentTranstactionStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
