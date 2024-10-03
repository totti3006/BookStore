using BookStore.Application.Interfaces.Services;
using BookStore.Application.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebAPI.Controllers
{
    public class CartController : AppControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartService _cartService;

        public CartController(ILogger<CartController> logger,
                              ICartService cartService)
        {
            _logger = logger;
            _cartService = cartService;
        }

        [HttpGet("get")]
        [Authorize(Roles = "user,admin")]
        public async Task<ActionResult<CartDetailDto>> GetCartDetail()
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            Guid userId = GetUserId();

            _logger.LogInformation($"User {userId} get cart detail");

            var carDetail = await _cartService.GetAll(userId);

            return Ok(carDetail);
        }

        [HttpPost("add")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> AddItemToCart([FromQuery] string bookId)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            if (!Guid.TryParse(bookId, out Guid _bookId))
            {
                return BadRequest("Invalid book Id");
            }

            var cart = new CartDto
            {
                UserId = GetUserId(),
                BookId = _bookId
            };

            await _cartService.AddToCart(cart);

            return Ok();
        }

        [HttpDelete("remove")]
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> RemoveItemFromCart([FromQuery] string cartId)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join("\n", ModelState.Values.SelectMany(value => value.Errors)
                                                                   .Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }

            if (!Guid.TryParse(cartId, out Guid _cartId))
            {
                return BadRequest("Invalid cart Id");
            }

            var cartItem = new CartItemRemoveDto
            {
                CartId = _cartId,
                UserId = GetUserId()
            };

            await _cartService.RemoveFromCart(cartItem);

            return Ok();
        }
    }
}
