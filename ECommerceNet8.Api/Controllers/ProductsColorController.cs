using ECommerceNet8.Infrastructure.Data.ProductModels;
using ECommerceNet8.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceNet8.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsColorController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ProductsColorController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet("GetAllProductsColor")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.productColors.ToListAsync());
        }

        [HttpGet("GetProductsColors")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            var productColor = await _context.productColors.FirstOrDefaultAsync(Ps => Ps.Id == Id);
            if (productColor == null) return BadRequest();

            return Ok(productColor);
        }

        [HttpPost("AddProductColor")]
        public async Task<IActionResult> Add(string Name)
        {
            if (Name == null)
                return BadRequest();
            var productColor = new ProductColor() { Name = Name };
            await _context.productColors.AddAsync(productColor);
            await _context.SaveChangesAsync();
            return Ok(productColor);
        }

        [HttpPut("EditProductColor")]
        public async Task<IActionResult> Update(int Id, string Name)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            if (Name == null)
                return BadRequest();
            var productColor = await _context.productColors.FirstOrDefaultAsync(Ps => Ps.Id == Id);
            if (productColor == null) return BadRequest();

            productColor.Name = Name;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteProductColor")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();
            var productColor = await _context.productColors.FirstOrDefaultAsync(Ps => Ps.Id == Id);
            if (productColor == null) return BadRequest();

            _context.productColors.Remove(productColor);
            return Ok();
        }

    }
}
