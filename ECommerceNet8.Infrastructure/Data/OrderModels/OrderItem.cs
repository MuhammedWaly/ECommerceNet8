using ECommerceNet8.Infrastructure.Data.OrderModels;
using ECommerceNet8.Infrastructure.Data.ProductModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerceNet8.Infrastructure.Data.OrderModels
{
    public class OrderItem
    {


        public int Id { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        public int ProductVariantId { get; set; }
        [JsonIgnore]
        public ProductVariant ProductVariant { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}
