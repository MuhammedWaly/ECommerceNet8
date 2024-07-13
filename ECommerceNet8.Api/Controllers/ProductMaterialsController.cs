using ECommerceNet8.Core.DTOS.ProductDtos.Request;
using ECommerceNet8.Core.Reposiatories.MaterialReposaitory;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceNet8.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMaterialsController : ControllerBase
    {
        private readonly IMaterialReposaitory _materialReposaitory;
        public ProductMaterialsController(IMaterialReposaitory materialReposaitory)
        {
            _materialReposaitory = materialReposaitory;
        }


        [HttpGet("GetAllProductMaterial")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _materialReposaitory.GetAllAsync());
        }

        [HttpGet("GetProductMaterialyById")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            return Ok(await _materialReposaitory.GetAllByIdAsync(Id));
        }

        [HttpPost("AddProductMaterial")]
        public async Task<IActionResult> Add(ProductMaterialRequest ProductMaterialDto)
        {
            if (ProductMaterialDto.Name == null)
                return BadRequest();

            return Ok(await _materialReposaitory.AddProductMaterial(ProductMaterialDto));
        }

        [HttpPut("EditProductMaterial")]
        public async Task<IActionResult> Update(ProductMaterialRequest ProductMaterialDto, int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            if (ProductMaterialDto.Name == null)
                return BadRequest();

            return Ok(await _materialReposaitory.UpdateProductMaterial(Id, ProductMaterialDto));
        }

        [HttpDelete("DeleteProductMaterial")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            return Ok(await _materialReposaitory.DeleteProductMaterial(Id));
        }
    }
}
