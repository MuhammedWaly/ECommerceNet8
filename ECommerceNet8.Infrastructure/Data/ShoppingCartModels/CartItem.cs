using ECommerceNet8.Infrastructure.Data.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Infrastructure.Data.ShoppingCartModels
{
    public class CartItem
    {
        public string Name {  get; set; }

        public string Description {  get; set; }

        public string Image {  get; set; }

        public int ProductVariantId {  get; set; }
        public ProductVariant ProductVariant {  get; set; }

        public int Quantity {  get; set; }

        public decimal Price {  get; set; } 
        public decimal TotalPrice {  get; set; }
    }
}
