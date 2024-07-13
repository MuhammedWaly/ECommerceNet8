using ECommerceNet8.Core.DTOS.ShoppingCartDto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ShoppingCartDto.Response
{
    public class Response_ShoppingCart
    {
        public int ShoppingCartId {  get; set; }
        public bool CanBeSold {  get; set; }
        public string Message {  get; set; }
        public decimal TotalPrice {  get; set; }

        public List<Model_CartItemReturn> ItemsCanBeSold { get; set; } = new List<Model_CartItemReturn>();
        public List<Model_CartItemReturn> ItemsCantBeSold { get; set; } = new List<Model_CartItemReturn>();
    }
}
