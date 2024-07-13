using ECommerceNet8.Core.DTOS.MainCatergoryDtos.Request;
using ECommerceNet8.Core.Reposiatories.MainCategoryReposaitory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceNet8.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainCategoriesController : ControllerBase
    {
        private readonly IMainCategory _mainCategory;

        public MainCategoriesController(IMainCategory mainCategory)
        {
            _mainCategory = mainCategory;
        }

        [HttpGet("GetAllMainCategories")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mainCategory.GetAllAsync());
        }
        
        [HttpGet("GetmainCategoryById")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            return Ok(await _mainCategory.GetByIdAsync(Id));
        }
        
        [HttpPost("AddCategory")]
        public async Task<IActionResult> Add( MainCategoryRequest mainCategoryDto)
        {
            if (mainCategoryDto.Name == null)
                return BadRequest();

            return Ok(await _mainCategory.AddMainCategory(mainCategoryDto));
        }
        
        [HttpPut("EditCategory")]
        public async Task<IActionResult> Update( MainCategoryRequest mainCategoryDto,int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            if (mainCategoryDto.Name == null)
                return BadRequest();

            return Ok(await _mainCategory.updateMainCategory(mainCategoryDto,Id));
        }
        
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == 0 || Id < 0)
                return BadRequest();

            return Ok(await _mainCategory.DeleteMainCategory(Id));
        }
    }
}
