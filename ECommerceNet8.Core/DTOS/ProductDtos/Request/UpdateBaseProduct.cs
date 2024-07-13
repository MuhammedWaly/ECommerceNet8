using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ProductDtos.Request
{
    public class UpdateBaseProduct
    {
       
        public string Name { get; set; }

        public string Description { get; set; }

        public int MainCategoreyId { get; set; }

        public int MaterialId { get; set; }

        public int ProductColorId { get; set; }

        public int ProductSizeId { get; set; }

        public int Quantity { get; set; }

        public decimal price { get; set; }

        public int Discount { get; set; }

        public IFormFile? Image { get; set; }
    }
}
