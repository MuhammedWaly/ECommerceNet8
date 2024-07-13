using ECommerceNet8.Core.DTOS.ProductDtos.Response;
using ECommerceNet8.Core.Reposiatories.OrderRepository;
using ECommerceNet8.Core.Reposiatories.RedisCartReposaitory;
using ECommerceNet8.Infrastructure.Constants;
using ECommerceNet8.Infrastructure.Data;
using ECommerceNet8.Infrastructure.Data.AuthModels;
using ECommerceNet8.Infrastructure.Data.OrderModels;
using ECommerceNet8.Infrastructure.Data.ProductModels;
using ECommerceNet8.Models.OrderModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceNet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;
        private readonly IRedisCartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public OrderController(IOrderRepository orderRepository, ApplicationDbContext context,
            IRedisCartService cartService, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            _orderRepository = orderRepository;
            _context = context;
            _cartService = cartService;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
        }



        [HttpGet("GetAllOrders")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).ToListAsync();
        }

        [HttpGet("GetUserOrders")]
        [Authorize]
        public async Task<ActionResult<List<Order>>> GetOrder()
        {
            var userId = HttpContext.User.FindFirstValue("uid");
            var user = await _userManager.FindByIdAsync(userId);

            var order = await _context.Orders.
                Include(o => o.OrderItems).ThenInclude(oi => oi.ProductVariant).
                Where(or => or.CustomerEmail == user.Email).ToListAsync();
                
                

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpGet("GetPdf")]
        public async Task<IActionResult> GetPdf(int OrderId)
        {
            var order = await _orderRepository.GetOrderForPdf(OrderId);
            if (order == null)
            {
                return NotFound("Check Unique Id");
            }

            var date = order.OrderDate.ToString();
            var dateNormalized = date.Replace("/", "_");
            string fileName = "PDF_" + dateNormalized + ".pdf";

            var provider = new FileExtensionContentTypeProvider();
            string filePath = order.PdfInfo.Path;
            string contentType = "application/octet-stream";
            byte[] fileBytes;

            fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, contentType, fileName);

        }

        #region
        //[HttpPost]
        //[Authorize]
        //public async Task<ActionResult<Order>> CreateOrder()
        //{
        //    string userId = HttpContext.User.FindFirstValue("uid");
        //    var user = await _userManager.FindByIdAsync(userId);
        //    var cart = await _cartService.GetCartAsync(userId);
        //    if (cart == null) return NotFound("Add Product to your cart frist");

        //    var UserAddress = await _context.Addresses.FirstOrDefaultAsync(ad => ad.ApplicationUserId == userId);
        //    if (UserAddress == null) return NotFound("Add Address frist");



        //    var order = new Order()
        //    {
        //        CustomerEmail = user.Email,
        //        Status = "Pending",
        //        OrderItems = new List<OrderItem>()

        //    };

        //    await _context.Orders.AddAsync(order);

        //    foreach (var item in cart.CartItems)
        //    {
        //        var orderitem = new OrderItem()
        //        {
        //            ProductVariantId = item.ProductVariantId,
        //            Quantity = item.Quantity,
        //            UnitPrice = item.Price,
        //            ProductVariant = item.ProductVariant,
        //            TotalPrice = item.Quantity * item.Price,
        //            Order = order,
        //            OrderId = order.Id,

        //        };
        //        await _context.OrdersItems.AddAsync(orderitem);
        //        order.OrderItems.Add(orderitem);

        //    }

        //    order.TotalAmount = cart.TotalPrice;



        //    PdfInfo pdfInfo = new PdfInfo();
        //    pdfInfo.OrderId = order.Id;
        //    pdfInfo.Name = "PDF info for " + order.Id + " order";
        //    pdfInfo.Added = DateTime.UtcNow;
        //    pdfInfo.Path = await _orderRepository.CreatePdf(order, userId);

        //    await _context.PdfInfos.AddAsync(pdfInfo);

        //    order.PdfInfo = pdfInfo;

        //    await _cartService.ClearCartAsync(userId);

        //    await _context.SaveChangesAsync();


        //    return Ok(order);
        //}


        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        //{
        //    if (id != order.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrder(int id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}


        #endregion 


        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }


        //[HttpPost("AddImage")]
        //public async Task<IActionResult> AddImage(IFormFile baseProduct)
        //{
        //    var fileExtension = Path.GetExtension(baseProduct.FileName);

        //    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ProductsImages");
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory("ProductsImages");
        //    }

        //    var UniqueFileName = Guid.NewGuid().ToString() + fileExtension;
        //    var filePath = Path.Combine(uploadsFolder, UniqueFileName);

        //    using (var Filestream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await baseProduct.CopyToAsync(Filestream);
        //    }

        //    var imageBases = new ImageBase
        //    {
        //        StaticPath = UniqueFileName,
        //        ImahePath = filePath,
        //        BaseProductId = 1

        //    };

        //    await _context.ImageBases.AddAsync(imageBases);
        //    await _context.SaveChangesAsync();

        //    return Ok();

        //}

    }


}

