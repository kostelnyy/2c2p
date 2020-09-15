using CCPP.WebApi.Tests.Integration.TestsInfrastructure;
using FluentAssertions;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CCPP.WebApi.Tests.Integration
{
    public class FileUploadShould : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public FileUploadShould(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        //TODO: test is not ready!!
        [Fact]
        public async Task SuccesfullyWriteValidCsvInput()
        {
            var fileContent = CreateFileContent("This is a dummy file", "test.csv");
            var response = await _client.PostAsync("/uploads", fileContent);

            response.IsSuccessStatusCode.Should().BeTrue();
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
