using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceNet8.Infrastructure.Data.ProductModels
{
    public class ProductVariant
    {
        public int Id { get; set; }

        public int BaseProductId { get; set; }
        [JsonIgnore]
        public BaseProduct BaseProduct { get; set; }

        public int ProductColorId {  get; set; }
        public ProductColor ProductColor { get; set; }

        public int ProductSizeId {  get; set; }
        public ProductSize ProductSize {  get; set; }

        public int Quantity {  get; set; }
    }
}
