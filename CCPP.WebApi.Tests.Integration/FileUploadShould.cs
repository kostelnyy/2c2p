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
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public FileUploadShould(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task SuccesfullyWriteValidCsvInput()
        {
            var repoMock = Substitute.For<IPaymentTranstactionsRepository>();

            var client = _factory.WithWebHostBuilder(b => {
                b.ConfigureTestServices(services =>
                {
                    services.AddSingleton(repoMock);
                });
            }).CreateClient();

            var content =
@"""Invoice0000001"",""1,000.00"",""USD"",""20/02/2019 12:33:16"",""Approved""
""Invoice0000002"",""300.00"",""USD"",""21/02/2019 02:04:59"",""Failed""
""Invoice0000003"",""40.00"",""EUR"",""01/02/2020 02:04:59"",""Finished""
";

            var fileContent = CreateFileContent(content, "test.csv");
            var response = await client.PostAsync("/uploads", fileContent);

            response.IsSuccessStatusCode.Should().BeTrue();
            await repoMock.Received().SaveChangesAsync();
            await repoMock
                .Received()
                .AddAsync(Arg.Is<IEnumerable<PaymentTranstaction>>(list => 
                    list.First().Id == "Invoice0000001" &&
                    list.First().Amount == 1000 &&
                    list.First().Currency == "USD" &&
                    list.First().TransactionDate == new DateTime(2019, 2, 20, 12, 33, 16) &&
                    list.First().Status == PaymentTranstactionStatus.Approved &&
                    list.Last().Status == PaymentTranstactionStatus.Done
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
