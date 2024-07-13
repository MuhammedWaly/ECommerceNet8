using ECommerceNet8.Core.DTOS.MainCatergoryDtos.Request;
using ECommerceNet8.Core.DTOS.MainCatergoryDtos.Response;
using ECommerceNet8.Infrastructure.Data;
using ECommerceNet8.Infrastructure.Data.ProductModels;
using Microsoft.EntityFrameworkCore;

namespace ECommerceNet8.Core.Reposiatories.MainCategoryReposaitory
{
    public class MainCategory : IMainCategory
    {
        public readonly ApplicationDbContext _context;

        public MainCategory(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<MainCategoryResponse> GetAllAsync()
        {
            return new MainCategoryResponse() 
            { IsSuccess = true, MainCategories = await _context.mainCategories.ToListAsync() };
        }

        public async Task<MainCategoryResponse> GetByIdAsync(int Id)
        {
            var mainCategory = await _context.mainCategories.FirstOrDefaultAsync(mc => mc.Id == Id);
            if (mainCategory == null)
                return new MainCategoryResponse() { IsSuccess = false };

            return new MainCategoryResponse()
            { IsSuccess = true, MainCategories = new List<MainCategorie>() { mainCategory } };
        }

        public async Task<MainCategoryResponse> updateMainCategory(MainCategoryRequest mainCategoryDto, int Id)
        {
            var maincategory = await _context.mainCategories.FirstOrDefaultAsync(mc => mc.Id == Id);
            if (maincategory == null)
                return new MainCategoryResponse() { IsSuccess = false };
            maincategory.Name = mainCategoryDto.Name;
            return new MainCategoryResponse()
            { IsSuccess = true };
        }

        public async Task<MainCategoryResponse> AddMainCategory(MainCategoryRequest mainCategoryDto)
        {
            var newcategory = new MainCategorie()
            {
                Name = mainCategoryDto.Name
            };

            await _context.mainCategories.AddAsync(newcategory);
            await _context.SaveChangesAsync();
            return new MainCategoryResponse()
            { IsSuccess = true };
        }

        public async Task<MainCategoryResponse> DeleteMainCategory(int Id)
        {
            var maincategory = await _context.mainCategories.FirstOrDefaultAsync(mc => mc.Id == Id);
            if (maincategory == null)
                return new MainCategoryResponse() { IsSuccess = false };

            _context.mainCategories.Remove(maincategory);
            return new MainCategoryResponse()
            { IsSuccess = true };
        }

       
    }
}
