

using ECommerceNet8.Infrastructure.Data.ProductModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceNet8.Core.DTOS.ProductDtos
{
    public class Model_BaseProductCustom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MainCategorie mainCategory { get; set; }
        public Material material { get; set; }

        public ICollection<Model_ProductVariantCustom> productVariants { get; set; }

        public ICollection<Model_BaseImageCustom> ImagesBases { get; set; }

        [Column(TypeName="decimal(18,2)")]
        public decimal Price { get; set; }
        public int Discount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}
