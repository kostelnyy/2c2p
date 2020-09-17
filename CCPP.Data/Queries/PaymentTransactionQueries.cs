using CCPP.Core.Domain;
using CCPP.Core.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPP.Data.Queries
{
    public class PaymentTransactionQueries : IPaymentTranstactionQueries
    {
        private readonly CcppDbContext _dbContext;

        public PaymentTransactionQueries(CcppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<PaymentTransactionDto>> GetTransactions(GetPaymentTransactionsQuery query)
        {

            var result = _dbContext.PaymentTranstactions
                .AsNoTracking();

            result = query.Currency is null ? result : result.Where(p => p.Currency == query.Currency);
            result = query.From is null ? result : result.Where(p => p.TransactionDate > query.From);
            result = query.To is null ? result : result.Where(p => p.TransactionDate < query.To);
            result = query.Status is null ? result : result.Where(p => p.Status == query.Status);

            var res = (await result.Select(x => new
            {
                x.Id,
                x.Amount,
                x.Currency,
                x.Status
            })
                .ToListAsync())
                .Select(p => new PaymentTransactionDto
                {
                    Id = p.Id,
                    Status = p.Status,
                    Payment = $"{p.Amount} {p.Currency}"
                });
            return res;
        }
    }
}
