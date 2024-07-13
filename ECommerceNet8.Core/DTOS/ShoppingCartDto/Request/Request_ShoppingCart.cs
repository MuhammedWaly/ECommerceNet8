using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ShoppingCartDto.Request
{
    public class Request_ShoppingCart
    {
        public int ProductVariantId {  get; set; }
        public int Quantity {  get; set; }
    }
}
