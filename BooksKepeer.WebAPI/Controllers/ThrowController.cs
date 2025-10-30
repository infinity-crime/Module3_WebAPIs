using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksKepeer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThrowController : ControllerBase
    {
        [HttpGet("generate-exception")]
        public IActionResult GetException()
        {
            throw new NotImplementedException();
        }
    }
}
