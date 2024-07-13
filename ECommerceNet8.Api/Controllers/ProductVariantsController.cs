using ECommerceNet8.Infrastructure.Data.ProductModels;
using ECommerceNet8.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceNet8.Core.DTOS.ProductVariantDtos;

namespace ECommerceNet8.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductVariant
        [HttpGet("GetAllProductVariant")]
        public async Task<ActionResult<IEnumerable<ProductVariantsResponseDTO>>> GetProductVariants()
        {
            var Productvariant = await _context.productVariants
                .Include(pv => pv.BaseProduct)
                .Include(pv => pv.ProductColor)
                .Include(pv => pv.ProductSize)
                .ToListAsync();
            var ProductvariantDto = new List<ProductVariantsResponseDTO>();

            foreach (var item in Productvariant)
            {
                ProductvariantDto.Add(new ProductVariantsResponseDTO()
                {
                    BaseProductId = item.BaseProductId,
                    ProductColorId = item.ProductColorId,
                    ProductColor = item.ProductColor.Name,
                    ProductSizeId = item.ProductSizeId,
                    ProductSize = item.ProductSize.Name,
                    Quantity = item.Quantity
                });
            }
            return Ok(ProductvariantDto);
        }

        // GET: api/ProductVariant
        [HttpGet("GetAllProductVariantOfBaseProduct")]
        public async Task<ActionResult<IEnumerable<ProductVariantsResponseDTO>>> GetProductVariantsOfBase(int BaseProductId)
        {
            var Productvariant =
                await _context.productVariants
                                      .Include(pv => pv.BaseProduct)
   .Include(pv => pv.ProductColor)
   .Include(pv => pv.ProductSize)
   .Where(pr => pr.BaseProductId == BaseProductId)
   .Select(item => new ProductVariantsResponseDTO
   {
       BaseProductId = item.BaseProductId,
       ProductColorId = item.ProductColorId,
       ProductColor = item.ProductColor.Name,
       ProductSizeId = item.ProductSizeId,
       ProductSize = item.ProductSize.Name,
       Quantity = item.Quantity
   })
   .ToListAsync();


            return Ok(Productvariant);

        }

        // GET: api/ProductVariant/5
        [HttpGet("GetProductVariant{id}")]
        public async Task<ActionResult<ProductVariantsResponseDTO>> GetProductVariant(int id)
        {
            var productVariant = await _context.productVariants
                .Include(pv => pv.BaseProduct)
                .Include(pv => pv.ProductColor)
                .Include(pv => pv.ProductSize)
                .FirstOrDefaultAsync(pv => pv.Id == id);

            if (productVariant == null)
            {
                return NotFound();
            }

            var ProductvariantDto = new ProductVariantsResponseDTO()
            {



                BaseProductId = productVariant.BaseProductId,
                ProductColorId = productVariant.ProductColorId,
                ProductColor = productVariant.ProductColor.Name,
                ProductSizeId = productVariant.ProductSizeId,
                ProductSize = productVariant.ProductSize.Name,
                Quantity = productVariant.Quantity
            };


            return ProductvariantDto;
        }

        // PUT: api/ProductVariant/5
        [HttpPut("EditProductVariant{id}")]
        public async Task<IActionResult> PutProductVariant(int id, ProductVariantsDTO productVariantDto)
        {
            if (id <= 0) return BadRequest();

            var OldPRoduct = await _context.productVariants.FirstOrDefaultAsync(pv => pv.Id == id);
            if (OldPRoduct == null) return NotFound();

            OldPRoduct.BaseProductId = productVariantDto.BaseProductId;
            OldPRoduct.ProductSizeId = productVariantDto.ProductSizeId;
            OldPRoduct.ProductColorId = productVariantDto.ProductColorId;
            OldPRoduct.Quantity = productVariantDto.Quantity;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/ProductVariant/5
        [HttpPut("EditProductVariantQty{id}")]
        public async Task<IActionResult> PutProductVariantQty(int ProductVariantId, int NewQty)
        {
            if (ProductVariantId <= 0 || NewQty < 0) return BadRequest();

            var OldPRoduct = await _context.productVariants.FirstOrDefaultAsync(pv => pv.Id == ProductVariantId);
            if (OldPRoduct == null) return NotFound();

            OldPRoduct.Quantity = NewQty;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/ProductVariant
        [HttpPost("AddProductVariant")]
        public async Task<ActionResult<ProductVariant>> PostProductVariant(ProductVariantsDTO productVariantDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var Baseproduct = await _context.BaseProducts.FirstOrDefaultAsync(p => p.Id == productVariantDto.BaseProductId);

            ProductVariant productVariant = new ProductVariant()
            {
                BaseProductId = productVariantDto.BaseProductId,
                ProductColorId = productVariantDto.ProductColorId,
                ProductSizeId = productVariantDto.ProductSizeId,
                Quantity = productVariantDto.Quantity,
                BaseProduct = Baseproduct

            };
            _context.productVariants.Add(productVariant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductVariant", new { id = productVariant.Id }, productVariant);
        }

        // DELETE: api/ProductVariant/5
        [HttpDelete("DeleteProductVariant{id}")]
        public async Task<IActionResult> DeleteProductVariant(int id)
        {
            var productVariant = await _context.productVariants.FindAsync(id);
            if (productVariant == null)
            {
                return NotFound();
            }

            _context.productVariants.Remove(productVariant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductVariantExists(int id)
        {
            return _context.productVariants.Any(e => e.Id == id);
        }


    }
}

