using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksKepeer.WebAPI.Controllers
{
    /// <summary>
    /// Контроллер, который выбрасывает исключение
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ThrowController : ControllerBase
    {
        /// <summary>
        /// Метод выброса исключения, для проверки кастомного middleware
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("generate-exception")]
        public IActionResult GetException()
        {
            throw new NotImplementedException();
        }
    }
}
