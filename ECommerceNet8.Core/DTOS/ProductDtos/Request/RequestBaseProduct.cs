using ECommerceNet8.Infrastructure.Data.ProductModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ProductDtos.Request
{
    public class RequestBaseProduct
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public int MainCategoreyId { get; set; }

        public int MaterialId { get; set; }


        public int Quantity { get; set; }

        public decimal price { get; set; }

        public int Discount { get; set; }

        public IFormFile Image { get; set; }
    }
}
