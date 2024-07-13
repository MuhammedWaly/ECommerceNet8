using ECommerceNet8.Infrastructure.Data.ProductModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ProductDtos.Response
{
    public class GetBaseProductsResponse
    {
        

        public string Name { get; set; }
        public string Description { get; set; }

        public int MainCategorieId { get; set; }
        public string Category { get; set; }

        public int MaterialId { get; set; }
        public string Material { get; set; }

        public string Image { get; set; }

        [Column(TypeName = "decimal (18,2)")]
        public decimal price { get; set; }

        public int Discount { get; set; }

        [Column(TypeName = "decimal (18,2)")]
        public decimal Totalprice { get; set; }
    }
}
