using CCPP.Core.FileParsers;
using CCPP.Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CCPP.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadsController : ControllerBase
    {
        private readonly IEnumerable<FileParser> _fileParsers;
        private readonly IPaymentTranstactionsRepository _repository;

        public UploadsController(IEnumerable<FileParser> fileParsers,
            IPaymentTranstactionsRepository repository)
        {
            _fileParsers = fileParsers;
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile upload)
        {
            var parserToUse = _fileParsers.FirstOrDefault(p => p.CanParseFile(upload.FileName));
            if (parserToUse is null)
            {
                var validExtenstions = _fileParsers.Select(p => p.Extension);
                return BadRequest($"File format not supported. Valid formats: {string.Join(", ", validExtenstions)}");
            }
            using var reader = new StreamReader(upload.OpenReadStream());
            var res = await reader.ReadToEndAsync();

            var result = parserToUse.ParseContent(res);

            await _repository.AddAsync(result);
            await _repository.SaveChangesAsync();

            return Ok();
        }
    }
}
