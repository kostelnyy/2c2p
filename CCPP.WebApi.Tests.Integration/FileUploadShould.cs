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
            _client = factory.WithWebHostBuilder(b => {
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

            var fileContent = CreateFileContent(content, "test.csv");
            var response = await _client.PostAsync("/uploads", fileContent);

            response.IsSuccessStatusCode.Should().BeTrue();
            await _repoMock.Received().SaveChangesAsync();
            await _repoMock
                .Received()
                .AddAsync(Arg.Is<IEnumerable<PaymentTransaction>>(list => 
                    list.First().Id == "Invoice0000001" &&
                    list.First().Amount == 1000 &&
                    list.First().Currency == "USD" &&
                    list.First().TransactionDate == new DateTime(2019, 2, 20, 12, 33, 16) &&
                    list.First().Status == PaymentTransactionStatus.A &&
                    list.Last().Status == PaymentTransactionStatus.D
                ));
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
            var fileContent = CreateFileContent(content, "test.xml");
            var response = await _client.PostAsync("/uploads", fileContent);

            response.IsSuccessStatusCode.Should().BeTrue();
            await _repoMock.Received().SaveChangesAsync();
            await _repoMock
                .Received()
                .AddAsync(Arg.Is<IEnumerable<PaymentTransaction>>(list =>
                    list.First().Id == "Inv00001" &&
                    list.First().Amount == 200 &&
                    list.First().Currency == "USD" &&
                    list.First().TransactionDate == new DateTime(2019, 1, 23, 13, 45, 10) &&
                    list.First().Status == PaymentTransactionStatus.D &&
                    list.Last().Id == "Inv00002" &&
                    list.Last().Amount == 10000 &&
                    list.Last().Currency == "EUR" &&
                    list.Last().TransactionDate == new DateTime(2019, 1, 24, 16, 09, 15) &&
                    list.Last().Status == PaymentTransactionStatus.R
                ));
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
            body.Should().Be("File format not supported. Valid formats: xml, csv");
        }

        private HttpContent CreateFileContent(string content, string fileName)
        {
            var multipart = new MultipartFormDataContent();
            multipart.Add(new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(content))), "upload", fileName);
            return multipart;
        }
    }
}
