using ECommerceNet8.Core.DTOS.ProductDtos.Request;
using ECommerceNet8.Core.Reposiatories.BaseProductsReposaitory;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceNet8.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBaseProducts _baseProducts;

        public ProductsController(IBaseProducts baseProducts)
        {
            _baseProducts = baseProducts;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAll(string? searchText, int? pageNum, int? MinPrice, int? MaxPrice)
        {
            var products = await _baseProducts.GetAllProducts(searchText, pageNum, MinPrice, MaxPrice);
            if (products == null) return BadRequest();

            return Ok(products);
        }
        
        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetById(int Id)
        {
            var product= await _baseProducts.GetProductById(Id);
            if (product == null) return BadRequest();

            return Ok(product);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> Add(RequestBaseProduct requestBaseProduct)
        {
            var addProduct = await _baseProducts.AddProduct(requestBaseProduct);
            if(!addProduct.IsSuccess)
                return BadRequest();
            return Ok(addProduct); 
           

        } 
        
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Update(UpdateBaseProduct BaseProduct, int Id)
        {
            var updateProduct = await _baseProducts.UpdateProduct(BaseProduct, Id);
            if(!updateProduct.IsSuccess)
                return BadRequest();
            return Ok(updateProduct);  
        }
        
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> Delete( int Id)
        {
            var DeleteProduct = await _baseProducts.DeleteProduct(Id);
            if(!DeleteProduct.IsSuccess)
                return BadRequest();
            return Ok(DeleteProduct);  
        }

    }
}
