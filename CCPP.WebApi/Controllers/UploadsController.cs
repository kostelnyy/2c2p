using CCPP.Core.FileParsers;
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

        public UploadsController(IEnumerable<FileParser> fileParsers)
        {
            _fileParsers = fileParsers;
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
            return Ok();
        }
    }
}
