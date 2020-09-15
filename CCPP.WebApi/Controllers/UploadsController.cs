using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CCPP.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile upload)
        {
            using var reader = new StreamReader(upload.OpenReadStream());
            var res = await reader.ReadToEndAsync();
            return Ok();
        }
    }
}
