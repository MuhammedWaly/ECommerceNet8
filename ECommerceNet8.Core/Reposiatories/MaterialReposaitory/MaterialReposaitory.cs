using ECommerceNet8.Core.DTOS.ProductDtos.Request;
using ECommerceNet8.Core.DTOS.ProductDtos.Response;
using ECommerceNet8.Infrastructure.Data;
using ECommerceNet8.Infrastructure.Data.ProductModels;
using Microsoft.EntityFrameworkCore;


namespace ECommerceNet8.Core.Reposiatories.MaterialReposaitory
{
    public class MaterialReposaitory : IMaterialReposaitory
    {
        private readonly ApplicationDbContext _context;

        public MaterialReposaitory(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductMaterialResponse> GetAllAsync()
        {
            var materials = await _context.Materials.ToListAsync();
            return new ProductMaterialResponse()
            {
                IsSuccess = true,
                Materials = materials
            };
        }

        public async Task<ProductMaterialResponse> GetAllByIdAsync(int materialId)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == materialId);
            if (material == null)
            {
                return new ProductMaterialResponse()
                {
                    IsSuccess = false,
                    Message = "No Material found"
                };
            }

            return new ProductMaterialResponse()
            {
                IsSuccess = true,
                Materials = new List<Infrastructure.Data.ProductModels.Material> { material }
            };
        }

        public async Task<ProductMaterialResponse> AddProductMaterial(ProductMaterialRequest materialDto)
        {
            var material = new Material()
            {
                Name = materialDto.Name,
            };

           await  _context.Materials.AddAsync(material);
            await _context.SaveChangesAsync();
            return new ProductMaterialResponse()
            {
                IsSuccess = true,
                Message = "Material Added Successfully"
            };
        }


        public async Task<ProductMaterialResponse> DeleteProductMaterial(int Id)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == Id);
            if (material == null)
                return new ProductMaterialResponse() { IsSuccess = false, Message = "no matrtial found" };
              _context.Materials.Remove(material);
            return new ProductMaterialResponse() { IsSuccess = true };
        }


        public async Task<ProductMaterialResponse> UpdateProductMaterial(int Id, ProductMaterialRequest materialDto)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == Id);
            if (material == null)
                return new ProductMaterialResponse() { IsSuccess = false, Message = "no matrtial found" };
            material.Name = materialDto.Name;
           await  _context.SaveChangesAsync();
            return new ProductMaterialResponse() { IsSuccess = true };
        }
    }
}
