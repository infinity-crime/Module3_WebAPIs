using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Contracts.Commands;

namespace BooksKepeer.Books.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IBus _bus;

        public OrdersController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost("order")]
        public async Task<IActionResult> PublishOrderMessage([FromBody] SubmitOrderCommand command)
        {
            await _bus.Publish(command);

            return Accepted();
        }
    }
}
