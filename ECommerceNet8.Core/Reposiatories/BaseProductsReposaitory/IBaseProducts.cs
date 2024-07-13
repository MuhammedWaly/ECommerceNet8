using ECommerceNet8.Core.DTOS.ProductDtos.Request;
using ECommerceNet8.Core.DTOS.ProductDtos.Response;
using ECommerceNet8.Infrastructure.Data.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Reposiatories.BaseProductsReposaitory
{
    public interface IBaseProducts
    {
        Task<BaseProductsPagination> GetAllProducts(string? searchText, int? pageNum, int? MinPrice, int? MaxPrice);
        Task<BaseProductById> GetProductById(int Id);

        Task<GeneralResponse> AddProduct(RequestBaseProduct baseProduct);

        Task<GeneralResponse> UpdateProduct(UpdateBaseProduct baseProduct,int Id);

        Task<GeneralResponse> DeleteProduct(int Id);
    }
}
