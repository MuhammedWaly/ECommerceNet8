using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceNet8.Infrastructure.Data.ProductModels
{
    public class ProductColor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public IEnumerable<ProductVariant> productVariants { get; set; }
    }
}
