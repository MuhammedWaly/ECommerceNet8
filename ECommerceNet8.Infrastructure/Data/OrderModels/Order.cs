using ECommerceNet8.Infrastructure.Data.ShoppingCartModels;
using ECommerceNet8.Models.OrderModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerceNet8.Infrastructure.Data.OrderModels
{
    public class Order
    {

        public int Id { get; set; }
        public DateTime OrderDate { get; set; } 
        public string CustomerEmail { get; set; }

       
        public string Status { get; set; }

        public PdfInfo PdfInfo { get; set; }


        [JsonIgnore]
        public ICollection<OrderItem> OrderItems { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }
        //{
        //    get
        //    {
        //        decimal TotalPrice = 0;
        //        foreach (var item in OrderItems)
        //        {
        //            TotalPrice += item.TotalPrice;
        //        }
        //        return TotalPrice;
        //    }
        //    set { }
        //}





    }
}
