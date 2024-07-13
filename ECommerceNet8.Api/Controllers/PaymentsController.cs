using ECommerceNet8.Core.DTOS.PaymentDTo;
using ECommerceNet8.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore;
using ECommerceNet8.Core.Reposiatories.RedisCartReposaitory;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Stripe;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using ECommerceNet8.Infrastructure.Data.AuthModels;
using ECommerceNet8.Infrastructure.Data.OrderModels;
using ECommerceNet8.Models.OrderModels;
using Stripe.Issuing;
using ECommerceNet8.Core.Reposiatories.OrderRepository;

namespace ECommerceNet8.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRedisCartService _cartService;
        private readonly ApplicationDbContext _context;
        private readonly StripeSettings _stripeSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepository _orderRepository;

        public PaymentsController(IConfiguration configuration, IRedisCartService cartService, ApplicationDbContext context,
            IOptions<StripeSettings> stripeSettings, UserManager<ApplicationUser> userManager,
            IOrderRepository orderRepository)
        {
            _configuration = configuration;
            _cartService = cartService;
            _context = context;
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        [Authorize]
        [HttpPost("CheckoutCart")]
        public async Task<ActionResult> CheckoutCart()
        {
            string userId = HttpContext.User.FindFirstValue("uid")!;
            var User = await _userManager.FindByIdAsync(userId);
           
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                return NotFound("");
            }

            var order = new Order()
            {
                CustomerEmail = User.Email,
                Status = "Pending",
                OrderItems = new List<OrderItem>()

            };

            await _context.Orders.AddAsync(order);

            foreach (var item in cart.CartItems)
            {
                var orderitem = new OrderItem()
                {
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    ProductVariant = item.ProductVariant,
                    TotalPrice = item.Quantity * item.Price,
                    //Order = order,
                    OrderId = order.Id,

                };
                await _context.OrdersItems.AddAsync(orderitem);
                order.OrderItems.Add(orderitem);

            }

            order.TotalAmount = cart.TotalPrice;



            PdfInfo pdfInfo = new PdfInfo();
            pdfInfo.OrderId = order.Id;
            pdfInfo.Name = "PDF info for " + order.Id + " order";
            pdfInfo.Added = DateTime.UtcNow;
            pdfInfo.Path = await _orderRepository.CreatePdf(order, userId);

            await _context.PdfInfos.AddAsync(pdfInfo);

            order.PdfInfo = pdfInfo;

            

            await _context.SaveChangesAsync();

            var options = new SessionCreateOptions
            {
                ClientReferenceId = order.Id.ToString(),
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cart.CartItems.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // Price in cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                            Description = item.Description,
                            //Images = new List<string> { item.Image }
                        },
                    },
                    Quantity = item.Quantity,
                   

                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/api/payments/success?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/api/payments/cancel",
                CustomerEmail = User.Email,
                 
            };

            await _cartService.ClearCartAsync(userId);
            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(new { sessionId = session.Id, publishableKey = _configuration["Stripe:PublishableKey"] });
        }

        [HttpGet("success")]
        public async Task<IActionResult> CheckoutSuccess(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);
            var order = await _context.Orders.FirstOrDefaultAsync(or => or.Id == int.Parse(session.ClientReferenceId));




            order.Status = "Paid";
            order.TotalAmount = session.AmountTotal / 100m; // Amount in dollars


           

            await _context.SaveChangesAsync();
           

            return Redirect("/success"); // Redirect to a success page
        }

        [HttpGet("cancel")]
        public IActionResult CheckoutCancel()
        {
            return Redirect("/cancel"); // Redirect to a cancel page
        }


    }
}

