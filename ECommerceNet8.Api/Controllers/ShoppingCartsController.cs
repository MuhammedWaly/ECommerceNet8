using ECommerceNet8.Core.DTOS.ShoppingCartDto.Request;
using ECommerceNet8.Core.Reposiatories.RedisCartReposaitory;
using ECommerceNet8.Infrastructure.Data;
using ECommerceNet8.Infrastructure.Data.ShoppingCartModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceNet8.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IRedisCartService _cartService;
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(IRedisCartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        [Authorize]
        [HttpGet("GetCart")]
        public async Task<ActionResult<ShoppingCart>> GetCart()
        {
            string userId = HttpContext.User.FindFirstValue("uid");
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("AddToCart")]
        public async Task<ActionResult<CartItem>> AddToCart([FromBody] Request_ShoppingCart cartItemDto)
        {
            string userId = HttpContext.User.FindFirstValue("uid");
            var productVar = await _context.productVariants.FirstOrDefaultAsync(pr => pr.Id == cartItemDto.ProductVariantId);
            var baseProduct =await _context.BaseProducts.FirstOrDefaultAsync(Bp => Bp.Id == productVar.BaseProductId);


            var cart = await _cartService.GetCartAsync(userId);
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductVariantId == cartItemDto.ProductVariantId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity;
                existingItem.TotalPrice = cartItemDto.Quantity * baseProduct.Totalprice;
            }
            else
            {
                var CartItems = new CartItem()
                {
                    ProductVariantId = cartItemDto.ProductVariantId,
                    Quantity = cartItemDto.Quantity,
                    Price = baseProduct.Totalprice,
                    Name = baseProduct.Name,
                    Description = baseProduct.Description,
                    TotalPrice = cartItemDto.Quantity * baseProduct.Totalprice
                    //Image = baseProduct.ImageBases.FirstOrDefault(im => im.BaseProductId == baseProduct.Id).ImahePath
                };

                cart.CartItems.Add(CartItems);
                
            }

            await _cartService.SaveCartAsync(cart);

            return CreatedAtAction(nameof(GetCart), new { userId = userId }, cart);
        }

        [HttpDelete("/RemoveFromCart/{itemId}")]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            string userId = HttpContext.User.FindFirstValue("uid");
            var cart = await _cartService.GetCartAsync(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductVariantId == itemId);

            if (cartItem == null)
            {
                return NotFound();
            }

            cart.CartItems.Remove(cartItem);
            await _cartService.SaveCartAsync(cart);

            return NoContent();
        }
    }
}



