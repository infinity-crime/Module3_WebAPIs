using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasks1and2_SimpleApiWithDI.Interfaces;

namespace Tasks1and2_SimpleApiWithDI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        private readonly ITimeService _timeService;

        public HelloController(ITimeService timeService) // просим DI дать экзепляр сервиса
        {
            _timeService = timeService;
        }

        [HttpGet]
        public IActionResult GetHello()
        {
            // чтобы вернуть сообщение в JSON, нам нужно вернуть именно объект, а не просто строку
            return Ok(new {message = "Hello! It is HelloConroller!"});
        }

        [HttpGet("time")]
        public IActionResult GetTime()
        {
            return Ok(_timeService.GetTimeUtc3());
        }
    }
}
