using ECommerceNet8.Infrastructure.Data.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceNet8.Infrastructure.Data.ShoppingCartModels
{
    public class ShoppingCart
    {
        public string ApplicationUserId {  get; set; }
        [JsonIgnore]
        public ApplicationUser ApplicationUser {  get; set; }

        public ICollection<CartItem> CartItems {  get; set; }

        public decimal TotalPrice 
        {
            get
            {
                decimal TotalPrice = 0;
                foreach (var item in CartItems)
                {
                    TotalPrice += item.TotalPrice;
                }
                return TotalPrice;
            }
        }
    }
}
