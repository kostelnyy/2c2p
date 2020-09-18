using CCPP.Core.FileParsers;
using CCPP.Core.Repository;
using CCPP.WebApi.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [MaxSizeFilter(1*1024*1024)]
        public async Task<IActionResult> Post(IFormFile upload)
        {
            var parserToUse = _fileParsers.FirstOrDefault(p => p.CanParseFile(upload.FileName));
            if (parserToUse is null)
            {
                var validExtenstions = _fileParsers.Select(p => p.Extension);
                return ValidationProblem($"File format not supported. Valid formats: {string.Join(", ", validExtenstions)}");
            }

            var (Result, Errors) = parserToUse.ParseContent(upload.OpenReadStream());

            if (Errors.Any())
            {
                foreach(var e in Errors)
                {
                    ModelState.AddModelError("ParsingError", e);
                }
                return ValidationProblem(ModelState);
            }

            await _repository.AddAsync(Result);
            await _repository.SaveChangesAsync();

            return Ok();
        }
    }
}
