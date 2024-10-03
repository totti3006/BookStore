using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebAPI.Controllers
{
    public class OrderController : AppControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost("place-order")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> CreateNewOrder(CreateOrderRequest createOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            var order = new CreateOrderDto
            {
                UserId = GetUserId(),
                Books = createOrderRequest.Books,
            };

            await _orderService.CreateOrder(order);

            return Ok();
        }

        [HttpGet("get")]
        [Authorize(Roles = "user, admin")]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderPaging([FromQuery] int page = 1,
                                                                                    [FromQuery] int pageSize = 5)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            Guid userId = GetUserId();

            var orderDetails = await _orderService.GetOrdersPaging(userId, page, pageSize);

            return Ok(orderDetails);
        }
    }
}
