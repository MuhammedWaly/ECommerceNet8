using ECommerceNet8.Core.DTOS.MainCatergoryDtos.Request;
using ECommerceNet8.Core.DTOS.MainCatergoryDtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Reposiatories.MainCategoryReposaitory
{
    public interface IMainCategory
    {
        Task<MainCategoryResponse> GetAllAsync();

        Task<MainCategoryResponse> GetByIdAsync(int Id);

        Task<MainCategoryResponse> AddMainCategory(MainCategoryRequest mainCategoryDto);

        Task<MainCategoryResponse> DeleteMainCategory(int Id);

        Task<MainCategoryResponse> updateMainCategory(MainCategoryRequest mainCategoryDto, int Id);
    }
}
