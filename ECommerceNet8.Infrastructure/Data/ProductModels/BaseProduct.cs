using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Infrastructure.Data.ProductModels
{
    public class BaseProduct
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int MainCategorieId { get; set; }
        public MainCategorie MainCategorie { get; set; }

        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public ICollection<ProductVariant> productVariants {  get; set; }
        public ICollection<ImageBase> ImageBases { get; set; }

        [Column(TypeName ="decimal (18,2)")]
        public decimal price {  get; set; }

        public int Discount { get; set; }

        [Column(TypeName ="decimal (18,2)")]
        public decimal Totalprice {  get; set; } 

        
    }
}
