using CCPP.WebApi.Tests.Integration.TestsInfrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CCPP.WebApi.Tests.Integration
{
    public class FileUploadShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup>
            _factory;

        public FileUploadShould(
            CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task SuccesfullyWriteValidCsvInput()
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file"))), "upload", "testdata.csv");
            var response = await _client.PostAsync("/uploads", content);

            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }
}
