using CCPP.Core.Domain;

namespace CCPP.Core.Queries
{
    public class PaymentTransactionDto
    {
        public string Id { get; set; }
        public string Payment { get; set; }
        public PaymentTransactionStatus Status { get; set; }
    }
}