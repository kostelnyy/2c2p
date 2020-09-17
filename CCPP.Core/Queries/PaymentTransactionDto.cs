namespace CCPP.Core.Queries
{
    public class PaymentTransactionDto
    {
        public string Id { get; set; }
        public string Payment { get; set; }
        public PaymentTransactionStatusDto Status { get; set; }
    }
}