using CCPP.Core.Domain;
using CCPP.Core.Repository;
using CCPP.WebApi.Tests.Integration.TestsInfrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CCPP.WebApi.Tests.Integration
{
    public class FileUploadShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly IPaymentTranstactionsRepository _repoMock;
        private readonly HttpClient _client;

        public FileUploadShould(CustomWebApplicationFactory<Startup> factory)
        {
            _repoMock = Substitute.For<IPaymentTranstactionsRepository>();
            _client = factory.WithWebHostBuilder(b =>
            {
                b.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_repoMock);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task SuccesfullyWriteValidCsvInput()
        {
            var content =
@"""Invoice0000001"",""1,000.00"",""USD"",""20/02/2019 12:33:16"",""Approved""
""Invoice0000002"",""300.00"",""USD"",""21/02/2019 02:04:59"",""Failed""
""Invoice0000003"",""40.00"",""EUR"",""01/02/2020 02:04:59"",""Finished""
";
            IEnumerable<PaymentTransaction> transaction = default;
            await _repoMock.AddAsync(Arg.Do<IEnumerable<PaymentTransaction>>(q => transaction = q));

            var fileContent = CreateFileContent(content, "test.csv");
            var response = await _client.PostAsync("/uploads", fileContent);

            response.IsSuccessStatusCode.Should().BeTrue();
            await _repoMock.Received().SaveChangesAsync();

            transaction.Should().BeEquivalentTo(new PaymentTransaction
            {
                Id = "Invoice0000001",
                Amount = 1000m,
                Currency = "USD",
                TransactionDate = new DateTime(2019, 2, 20, 12, 33, 16),
                Status = PaymentTransactionStatus.A
            },
            new PaymentTransaction
            {
                Id = "Invoice0000002",
                Amount = 300m,
                Currency = "USD",
                TransactionDate = new DateTime(2019, 2, 21, 2, 4, 59),
                Status = PaymentTransactionStatus.R
            },
            new PaymentTransaction
            {
                Id = "Invoice0000003",
                Amount = 40m,
                Currency = "EUR",
                TransactionDate = new DateTime(2020, 2, 1, 2, 4, 59),
                Status = PaymentTransactionStatus.D
            });
        }

        [Fact]
        public async Task ReturnBadRequestWithValidationDetailsIfSomeFieldIsEmpty()
        {
            var content =
@"""Invoice0000001"","""",""USD"",""20/02/2019 12:33:16"",""Approved""
""Invoice0000002"",""300.00"",""USD"",""21/02/2019 02:04:59"",""Failed""
""Invoice0000003"",""500"",""USD"","""",""Approved""
";

            var fileContent = CreateFileContent(content, "test.csv");
            var response = await _client.PostAsync("/uploads", fileContent);

            response.IsSuccessStatusCode.Should().BeFalse();
            await _repoMock.DidNotReceiveWithAnyArgs().SaveChangesAsync();
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Contain("\\\"Invoice0000001\\\",\\\"\\\",\\\"USD\\\",\\\"20/02/2019 12:33:16\\\",\\\"Approved\\\"");
            result.Should().Contain("\\\"Invoice0000003\\\",\\\"500\\\",\\\"USD\\\",\\\"\\\",\\\"Approved\\\"");
        }

        [Fact]
        public async Task SuccesfullyWriteValidXmlInput()
        {
            var content =
@"
<Transactions>
	<Transaction id=""Inv00001"">
		<TransactionDate>2019-01-23T13:45:10</TransactionDate>
		<PaymentDetails>
			<Amount>200.00</Amount>
			<CurrencyCode>USD</CurrencyCode>
		</PaymentDetails>
		<Status>Done</Status>
	</Transaction>
	<Transaction id=""Inv00002"">
		<TransactionDate>2019-01-24T16:09:15</TransactionDate>
		<PaymentDetails>
			<Amount>10000.00</Amount>
			<CurrencyCode>EUR</CurrencyCode>
		</PaymentDetails>
		<Status>Rejected</Status>
	</Transaction>
</Transactions>	 
";
            IEnumerable<PaymentTransaction> transaction = default;
            await _repoMock.AddAsync(Arg.Do<IEnumerable<PaymentTransaction>>(q => transaction = q));

            var fileContent = CreateFileContent(content, "test.xml");
            var response = await _client.PostAsync("/uploads", fileContent);

            response.IsSuccessStatusCode.Should().BeTrue();
            await _repoMock.Received().SaveChangesAsync();

            transaction.Should().BeEquivalentTo(new PaymentTransaction
            {
                Id = "Inv00001",
                Amount = 200m,
                Currency = "USD",
                TransactionDate = new DateTime(2019, 1, 23, 13, 45, 10),
                Status = PaymentTransactionStatus.D
            },
            new PaymentTransaction
            {
                Id = "Inv00002",
                Amount = 10000m,
                Currency = "EUR",
                TransactionDate = new DateTime(2019, 1, 24, 16, 9, 15),
                Status = PaymentTransactionStatus.R
            });
        }

        [Theory]
        [InlineData("test")]
        [InlineData("1")]
        [InlineData("test.json")]

        public async Task ReturnBadRequestForUnknownOrUnsupportedFormat(string fileName)
        {
            var fileContent = CreateFileContent("This is a dummy file", fileName);
            var response = await _client.PostAsync("/uploads", fileContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("File format not supported. Valid formats: .xml, .csv");
        }

        [Fact]
        public async Task ReturnBadRequestForFilesBigger1MB()
        {
            var fileContent = CreateFileContent("This is a dummy file", "test.csv");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_client.BaseAddress + "uploads"),
                Content = fileContent
            };
            request.Content.Headers.Add("content-length", (2 * 1024 * 1024).ToString());
            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("Request body size should not exceed 1048576 bytes.");
        }

        private HttpContent CreateFileContent(string content, string fileName)
        {
            var multipart = new MultipartFormDataContent();
            multipart.Add(new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(content))), "upload", fileName);
            return multipart;
        }
    }
}
