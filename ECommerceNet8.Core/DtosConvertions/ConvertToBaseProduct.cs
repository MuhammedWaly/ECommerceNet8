using ECommerceNet8.Core.DTOS.ProductDtos.Request;
using ECommerceNet8.Infrastructure.Data.ProductModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DtosConvertions
{
    public static class ConvertToBaseProduct
    {


        public static BaseProduct ConvertToBaseProducts(this RequestBaseProduct requestBaseProduct, List<ImageBase> imagebases)
        {
            decimal TotalPrice;
            decimal decimalTotalPrice;

            if (requestBaseProduct.Discount > 0)
            {
                TotalPrice = requestBaseProduct.price - (requestBaseProduct.price * requestBaseProduct.Discount / 100);
                decimalTotalPrice = decimal.Round(TotalPrice, 2);
            }
            else
            {
                TotalPrice = requestBaseProduct.price;
                decimalTotalPrice = decimal.Round(TotalPrice, 2);
            }


          

            var baseProduct = new BaseProduct()
            {
                Name = requestBaseProduct.Name,
                Description = requestBaseProduct.Description,
                Discount = requestBaseProduct.Discount,
                price = requestBaseProduct.price,
                MainCategorieId = requestBaseProduct.MainCategoreyId,
                MaterialId = requestBaseProduct.MaterialId,
                Totalprice = decimalTotalPrice,
                ImageBases = imagebases,



            };

            
            return baseProduct;
        }
    }
}
