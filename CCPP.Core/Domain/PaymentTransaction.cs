using System;

namespace CCPP.Core.Domain
{
    public class PaymentTransaction
    {
        private string _id;
        private string _currency;

        public string Id
        {
            get => _id;
            set => _id = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Id can not be empty") : value;
        }

        public decimal Amount { get; set; }

        public string Currency
        {
            get => _currency;
            set => _currency = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Currency can not be empty") : value;
        }

        public PaymentTransactionStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
