using ECommerceNet8.Infrastructure.Data;
using ECommerceNet8.Infrastructure.Data.ProductModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceNet8.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsSizeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsSizeController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        [HttpGet("GetAllProductsSize")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.productSizes.ToListAsync());
        }

        [HttpGet("GetProductsSize")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            var PrSize = await _context.productSizes.FirstOrDefaultAsync(Ps => Ps.Id == Id);
            if (PrSize == null) return BadRequest();

            return Ok(PrSize);
        }

        [HttpPost("AddProductSize")]
        public async Task<IActionResult> Add( string Name)
        {
            if (Name == null)
                return BadRequest();
            var productSize = new ProductSize() { Name = Name };
            await _context.productSizes.AddAsync(productSize);
           await _context.SaveChangesAsync();
            return Ok(productSize);
        }

        [HttpPut("EditProductMaterial")]
        public async Task<IActionResult> Update(int Id, string Name)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            if (Name == null)
                return BadRequest();
            var OldPrSize = await _context.productSizes.FirstOrDefaultAsync(Ps => Ps.Id == Id);
            if (OldPrSize == null) return BadRequest();

            OldPrSize.Name = Name;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteProductMaterial")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();
            var OldPrSize = await _context.productSizes.FirstOrDefaultAsync(Ps => Ps.Id == Id);
            if (OldPrSize == null) return BadRequest();

            _context.productSizes.Remove(OldPrSize);
            return Ok();
        }

    }
}
