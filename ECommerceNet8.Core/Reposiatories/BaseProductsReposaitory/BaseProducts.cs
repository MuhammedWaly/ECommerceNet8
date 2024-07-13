using Azure;
using ECommerceNet8.Core.DTOS.ProductDtos.Request;
using ECommerceNet8.Core.DTOS.ProductDtos.Response;
using ECommerceNet8.Core.DtosConvertions;
using ECommerceNet8.Infrastructure.Data;
using ECommerceNet8.Infrastructure.Data.ProductModels;
using iText.Commons.Actions.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Reposiatories.BaseProductsReposaitory
{
    public class BaseProducts : IBaseProducts
    {
        private readonly ApplicationDbContext _context;
        private string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BaseProducts(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }



        public async Task<BaseProductsPagination> GetAllProducts(string? searchText, int? pageNum, int? MinPrice, int? MaxPrice)
        {
            IQueryable<BaseProduct> query = _context.BaseProducts
                .Include(p => p.Material)
                .Include(p => p.MainCategorie)
                .Include(p => p.productVariants).ThenInclude(p => p.ProductColor)
                .Include(p => p.productVariants).ThenInclude(p => p.ProductSize)
                .Include(p => p.ImageBases);

            if (searchText != null)
            {
                query = query.Where(x => x.Name.Contains(searchText) || x.Description.Contains(searchText));
            }


            if (MinPrice != null)
            {
                query = query.Where(x => x.price >= MinPrice);
            }

            if (MaxPrice != null)
            {
                query = query.Where(x => x.price <= MaxPrice);
            }

            if (pageNum == null || pageNum < 1)
            {
                pageNum = 1;
            }
            int pageSize = 5;
            int TotalPages = 0;
            decimal count = _context.BaseProducts.Count();
            TotalPages = (int)Math.Ceiling(count / pageSize);

            query = query
               .Skip((int)(pageNum - 1) * pageSize)
               .Take(pageSize);

            var products = await query.ToListAsync();

            var ProductDto = new List<GetBaseProductsResponse>();
            foreach (var item in products)
            {


                ProductDto.Add(new GetBaseProductsResponse()
                {
                    Name = item.Name,
                    Description = item.Description,
                    Material = item.Material.Name,
                    MaterialId = item.MaterialId,
                    Category = item.MainCategorie.Name,
                    MainCategorieId = item.MainCategorieId,
                    Image = item.ImageBases.FirstOrDefault(im => im.BaseProductId == item.Id).ImahePath,
                    price = item.price,
                    Discount = item.Discount,
                    Totalprice = item.Totalprice

                });
            }


            var response = new BaseProductsPagination
            {
                Products = ProductDto,
                TotalPages = TotalPages,
                PageNumber = pageNum,
                IsSuccess = true
            };
            return response;
        }


        public async Task<BaseProductById> GetProductById(int Id)
        {
            if (Id <= 0) return new BaseProductById() { IsSuccess = false, Message = "Invalid Id" };

            var product = await _context.BaseProducts.FirstOrDefaultAsync(p => p.Id == Id);
            if (product == null) return new BaseProductById() { IsSuccess = false, Message = "No Product found" };

            if (product == null) return new BaseProductById() { IsSuccess = false, Message = "No Product found" };
            var ProductResp = new GetBaseProductsResponse()
            {
                Name = product.Name,
                Description = product.Description,
                Material = product.Material.Name,
                MaterialId = product.MaterialId,
                Category = product.MainCategorie.Name,
                MainCategorieId = product.MainCategorieId,
                Image = product.ImageBases.FirstOrDefault(im => im.BaseProductId == product.Id).ImahePath,
                price = product.price,
                Discount = product.Discount,
                Totalprice = product.Totalprice
            };
            return new BaseProductById() { IsSuccess = true, baseProduct = ProductResp };
        }


        public async Task<GeneralResponse> AddProduct(RequestBaseProduct baseProduct)
        {
            var fileExtension = Path.GetExtension(baseProduct.Image.FileName);
            if (!AllowedExtensions.Contains(fileExtension.ToLower()))
                return new GeneralResponse() { IsSuccess = false, Message = "Not supported" };

            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ProductsImages");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory("ProductsImages");
            }

            var UniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, UniqueFileName);

            using (var Filestream = new FileStream(filePath, FileMode.Create))
            {
                await baseProduct.Image.CopyToAsync(Filestream);
            }

            var imageBases = new List<ImageBase>
        {
            new ImageBase
            {
                StaticPath = UniqueFileName,
                ImahePath = filePath,
                
                
            }
        };

            var addedProduct = baseProduct.ConvertToBaseProducts(imageBases);
            foreach (var image in imageBases)
            {
                image.BaseProductId = addedProduct.Id;
            }


            await _context.BaseProducts.AddAsync(addedProduct);
            await _context.SaveChangesAsync();
            return new GeneralResponse() { IsSuccess = true, Message = "Product Added Successfully" };
        }

        public async Task<GeneralResponse> UpdateProduct(UpdateBaseProduct baseProduct, int Id)
        {
            if (Id <= 0) return new GeneralResponse() { IsSuccess = false, Message = "Invalid Id" };
            var OldProduct = await _context.BaseProducts.FirstOrDefaultAsync(p => p.Id == Id);
            if (OldProduct == null) return new GeneralResponse() { IsSuccess = false, Message = "No product found with this Id" };

            decimal TotalPrice;
            decimal decimalTotalPrice;

            if (baseProduct.Discount > 0)
            {
                TotalPrice = baseProduct.price - (baseProduct.price * baseProduct.Discount / 100);
                decimalTotalPrice = decimal.Round(TotalPrice, 2);
            }
            else
            {
                TotalPrice = baseProduct.price;
                decimalTotalPrice = decimal.Round(TotalPrice, 2);
            }


            if (baseProduct.Image != null)
            {
                var fileExtension = Path.GetExtension(baseProduct.Image.FileName);
                if (!AllowedExtensions.Contains(fileExtension.ToLower()))
                    return new GeneralResponse() { IsSuccess = false, Message = "Not supported" };

                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ProductsImages");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory("ProductsImages");
                }

                var UniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(uploadsFolder, UniqueFileName);

                using (var Filestream = new FileStream(filePath, FileMode.Create))
                {
                    await baseProduct.Image.CopyToAsync(Filestream);
                }

                var imageBases = new List<ImageBase>
                {
                    new ImageBase
                    {
                      StaticPath = UniqueFileName,
                      ImahePath = filePath
                    }
                };
                OldProduct.ImageBases = imageBases;
            }

            var productVariant = new List<ProductVariant>()
            {
                 new ProductVariant
                 {
                     ProductColorId = baseProduct.ProductColorId,
                     ProductSizeId = baseProduct.ProductSizeId,
                     Quantity = baseProduct.Quantity,
                 }
            };

            OldProduct.Name = baseProduct.Name;
            OldProduct.Description = baseProduct.Description;
            OldProduct.Discount = baseProduct.Discount;
            OldProduct.price = baseProduct.price;
            OldProduct.MainCategorieId = baseProduct.MainCategoreyId;
            OldProduct.MaterialId = baseProduct.MaterialId;
            OldProduct.Totalprice = decimalTotalPrice;
            OldProduct.productVariants = productVariant;

            await _context.SaveChangesAsync();
            return new GeneralResponse() { IsSuccess = true, Message = "Product Updated Successfully" };

        }

        public async Task<GeneralResponse> DeleteProduct(int Id)
        {
            if (Id <= 0) return new GeneralResponse() { IsSuccess = false, Message = "Invalid Id" };
            var OldProduct = await _context.BaseProducts.FirstOrDefaultAsync(p => p.Id == Id);
            if (OldProduct == null) return new GeneralResponse() { IsSuccess = false, Message = "No product found with this Id" };

            _context.BaseProducts.Remove(OldProduct);

            return new GeneralResponse() { IsSuccess = true, Message = "Product deleted succefully" };
        }
    }
}
