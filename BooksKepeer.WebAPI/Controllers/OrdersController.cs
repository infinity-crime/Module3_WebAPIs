using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Contracts.Commands;
using Orders.OrderWorkerService.Services;

namespace BooksKepeer.Books.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IOrderService _orderService;

        public OrdersController(IBus bus, IOrderService orderService)
        {
            _bus = bus;
            _orderService = orderService;
        }

        [HttpGet("/all-orders")]
        public IActionResult GetAll()
        {
            return Ok(_orderService.GetAllOrders());
        }

        [HttpPost("order")]
        public async Task<IActionResult> PublishOrderMessage([FromBody] SubmitOrderCommand command)
        {
            await _bus.Publish(command);

            return Accepted();
        }
    }
}
