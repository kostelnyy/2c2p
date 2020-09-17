using CCPP.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCPP.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IPaymentTranstactionQueries _transtactionQueries;

        public TransactionsController(IPaymentTranstactionQueries transtactionQueries)
        {
            _transtactionQueries = transtactionQueries;
        }
        [HttpGet]
        public async Task<IEnumerable<PaymentTransactionDto>> Get([FromQuery]GetPaymentTransactionsQuery query)
        {
            var result = await _transtactionQueries.GetTransactions(query);
            return result;
        }

    }
}
