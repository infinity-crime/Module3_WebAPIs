using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Tasks1and2_SimpleApiWithDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHello()
        {
            // чтобы вернуть сообщение в JSON, нам нужно вернуть именно объект, а не просто строку
            return Ok(new {message = "Hello! It is HelloConroller!"});
        }
    }
}
