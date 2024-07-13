using ECommerceNet8.Core.DTOS.ProductDtos.Request;
using ECommerceNet8.Core.DTOS.ProductDtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Reposiatories.MaterialReposaitory
{
    public interface IMaterialReposaitory
    {
        Task<ProductMaterialResponse> GetAllAsync ();

        Task<ProductMaterialResponse> GetAllByIdAsync (int materialId);

        Task<ProductMaterialResponse> AddProductMaterial (ProductMaterialRequest materialDto);

        Task<ProductMaterialResponse> UpdateProductMaterial (int Id,ProductMaterialRequest materialDto);

        Task<ProductMaterialResponse> DeleteProductMaterial (int Id);
    }
}
