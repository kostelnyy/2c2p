using CCPP.Core.Domain;
using CCPP.Core.Queries;
using CCPP.WebApi.Tests.Integration.TestsInfrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CCPP.WebApi.Tests.Integration
{
    public class GetTransactionsEndpointShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly IPaymentTranstactionQueries _queriesMock;
        private readonly HttpClient _client;

        public GetTransactionsEndpointShould(CustomWebApplicationFactory<Startup> factory)
        {
            _queriesMock = Substitute.For<IPaymentTranstactionQueries>();

            _client = factory.WithWebHostBuilder(b => {
                b.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_queriesMock);
                });
            }).CreateClient();
        }

        [Theory]
        [InlineData("/transactions", null, null, null, null)]
        [InlineData("/transactions?from=2020-03-12&to=2020-03-14&currency=EUR&status=A", 
            "2020-03-12", "2020-03-14", "EUR", PaymentTransactionStatus.A)]
        public async Task QueryDataProperlyByParams(string url, string from, string to, string currency, PaymentTransactionStatus? status)
        {
            GetPaymentTransactionsQuery query = default;
            await _queriesMock.GetTransactions(Arg.Do<GetPaymentTransactionsQuery>(q => query = q));
            var result = await _client.GetAsync(url);

            query.Should().BeEquivalentTo(new GetPaymentTransactionsQuery
            {
                Currency = currency,
                From = from is null? (DateTime?)null : DateTime.Parse(from),
                To = to is null ? (DateTime?)null : DateTime.Parse(to),
                Status = status
            });
        }
    }
}

