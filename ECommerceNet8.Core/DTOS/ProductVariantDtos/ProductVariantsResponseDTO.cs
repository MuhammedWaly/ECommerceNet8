using ECommerceNet8.Infrastructure.Data.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ProductVariantDtos
{
    public class ProductVariantsResponseDTO
    {
        public int BaseProductId { get; set; }

        public int ProductColorId { get; set; }
        public string ProductColor { get; set; }

        public int ProductSizeId { get; set; }
        public string ProductSize { get; set; }


        public int Quantity { get; set; }
    }
}
